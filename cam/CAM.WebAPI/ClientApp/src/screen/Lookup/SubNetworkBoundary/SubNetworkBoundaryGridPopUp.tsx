import React, { useState, useEffect, SetStateAction } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import {
  SelectFilterType,
  SelectGridType,
} from "../../../Hook/CommonRenderGrid/GridRender";
import { useSelector } from "react-redux";
import { RootState } from "../../../Redux/Store/rootStore";
import { useFilterTableCrud } from "../../../Hook/useFilterTableCrud";
import { RenderDetail } from "../../../Model/Common";

import {
  SubNetworkBoundaryGridDto,
  SubNetworkBoundaryQueryDto,
} from "../../../Model/LookUp/SubnetworkBoundry";
import { GetFilterColumSubNetworkBoundary } from "../../../Redux/Action/LookUp/SubNetworkBoundary/SubNetworkBoundryGridAction";
import { Dropdown } from "react-bootstrap";
import { GetProductNameByVodafoneName } from "../../../Redux/Action/LookUp/VodafoneName/VodafoneNameCommonAction";
import { toggleState } from "../../../Hook/Common";

interface Props {
  action: {
    onDelete(id: number | undefined): any;
    Edit(id: number | undefined): any;
    Filter(obj: SetStateAction<SubNetworkBoundaryQueryDto> | undefined): any;
  };
  data: SubNetworkBoundaryGridDto[] | undefined;
  pagination: SubNetworkBoundaryQueryDto | undefined;
  orphanColor?: boolean;
  renderGrid: RenderDetail[];
  readonly?: boolean;
}

const SubNetworkBoundaryGridPopUp: React.FC<Props> = (props) => {
  const [data, setData] = useState<SubNetworkBoundaryGridDto[] | undefined>([]);
  const [isVisibleVodafoneBubble, setIsVisibleVodafoneBubble] = useState(0);
  const [relatedVodafoneNames, setRelatedVodafoneNames] = useState([]);
  const getFiltersData = (state: RootState) =>
    state.subNetworkBoundaryGridReducer.filter;
  let filterData = useSelector(getFiltersData);
  const {
    filtriAttivi,
    isVisibleFiltri,
    setIsVisibleFiltri,
    resetFilter,
    closeAll,
    setDateToChildren,
    orderBy,
    resetFilterDate,
    getFilters,
    updateCount,
    getFiltriAttivi,
    count,
    checkFilterinValue,
    checkFilterDateinValue,
    isVisibleFiltriString,
    setIsVisibleFiltriString,
  } = useFilterTableCrud<SubNetworkBoundaryQueryDto>(
    props.action.Filter,
    GetFilterColumSubNetworkBoundary,
    props.pagination
  );

  //CARICAMENTO INIZIALE
  useEffect(() => {
    setData(props.data);
  }, []);

  ///UPDATE DATA
  useEffect(() => {
    setData(props?.data);
  }, [props.data]);

  //CHIAMATA AL PARENT AL CAMBIO FILTRI
  useEffect(() => {
    props.action.Filter(filtriAttivi);
  }, [filtriAttivi]);

  const thAction = {
    checkFilter: checkFilterinValue,
    settingVisibility: setIsVisibleFiltriString,
    resetFilter: resetFilter,
  };
  const actionFilterCK = {
    closeAll,
    updateCount,
    getFiltriAttivi,
    orderBy,
    getFilters,
  };
  const actionFilterDate = { closeAll, setDateToChildren, orderBy };
  const thActionDate = {
    checkFilter: checkFilterDateinValue,
    settingVisibility: setIsVisibleFiltriString,
    resetFilter: resetFilterDate,
  };
  const getRelatedVodafoneNames = async (vodafoneId, index) => {
    const result = await GetProductNameByVodafoneName(vodafoneId);
    setRelatedVodafoneNames(result);
    setIsVisibleVodafoneBubble(toggleState(index + 1, isVisibleVodafoneBubble));
  };
  const closeVodafoneBubble = () => {
    setIsVisibleVodafoneBubble(0);
  };
  return (
    <div className="listaApparatiContainer mt-3 row mx-0 col-12 p-0 d-flex justify-content-center">
      <div className="col-12 py-3">
        <table className="w-100 table-responsive">
          <thead>
            <tr className="intestazione">
              {props.renderGrid
                .sort((a, b) => a.order - b.order)
                .filter((x) => x.show)
                .map((item, i) =>
                  SelectFilterType(
                    item.propertyName,
                    item.type,
                    props.pagination?.isSortAscending,
                    filtriAttivi,
                    actionFilterDate,
                    props.pagination?.sortBy,
                    filterData,
                    count,
                    actionFilterCK,
                    thAction,
                    thActionDate,
                    isVisibleFiltriString,
                    undefined,
                    undefined,
                    undefined
                  )
                )}
               {props.renderGrid && props.renderGrid.length ? (
                <th className="customWidth"></th>
              ) : (
                ""
              )}
            </tr>
          </thead>
          <tbody>
            {data &&
              data?.map((item, index) => (
                <tr
                  key={item.subNetworkBoundaryId}
                  className={`dati ${
                    props?.orphanColor && item.orphan ? "orphan" : null
                  }`}
                >
                  {props.renderGrid
                    .sort((a, b) => a.order - b.order)
                    .filter((x) => x.show)
                    .map((td, i) =>
                      td.propertyName === "vodafoneName" ? (
                        <td
                          className="majorHardware"
                          key={`${td.propertyName}${i}`}
                        >
                          <div
                            className="majorHardware"
                            onClick={() =>
                              getRelatedVodafoneNames(
                                item["vodafoneNameId"],
                                index
                              )
                            }
                          >
                            {item?.vodafoneName != undefined ? (
                              <a
                                className=""
                                dangerouslySetInnerHTML={{
                                  __html: item.vodafoneName ?? "---",
                                }}
                              ></a>
                            ) : (
                              "---"
                            )}
                          </div>
                          {isVisibleVodafoneBubble == index + 1 ? (
                            <div
                              className="bubbleMenu"
                              onMouseLeave={closeVodafoneBubble}
                            >
                              <div className="triangleBubbleTop"></div>
                              <div className="col-12 row mx-0 px-2 my-2">
                                <nav className="nav flex-column">
                                  {item?.vodafoneName != undefined ? (
                                    <label
                                      style={{ cursor: "pointer" }}
                                      className="mb-0  text-white"
                                    >
                                      <ol>
                                        {relatedVodafoneNames &&
                                          relatedVodafoneNames?.map(
                                            (vdnames) => <li>{vdnames}</li>
                                          )}
                                      </ol>
                                    </label>
                                  ) : null}
                                </nav>
                              </div>
                            </div>
                          ) : null}
                        </td>
                      ) : (
                        SelectGridType(
                          item[td.propertyName],
                          td.propertyName,
                          td.type
                        )
                      )
                    )}
                  {!props.readonly && (
                    <td className="actions">
                      <Dropdown className="d-inline mx-2">
                        <Dropdown.Toggle id="dropdown-autoclose-true">
                          <img src={require("../../../img/options_dots.png")} />
                        </Dropdown.Toggle>

                        <Dropdown.Menu>
                          <>
                            <Dropdown.Item
                              disabled={item.default === true}
                              onClick={() =>
                                props.action.Edit(item.subNetworkBoundaryId)
                              }
                            >
                              Edit
                            </Dropdown.Item>
                            <Dropdown.Item
                              disabled={item.default === true}
                              onClick={() =>
                                props.action.onDelete(item.subNetworkBoundaryId)
                              }
                            >
                              Delete
                            </Dropdown.Item>
                          </>
                        </Dropdown.Menu>
                      </Dropdown>
                      {/* <div className="d-flex flex-row justify-content-end">
                      <button
                        type="button"
                        className="btn btn-link "
                        onClick={() =>
                          props.action.Edit(item.subNetworkBoundaryId)
                        }
                      >
                        <img
                          className="btnEdit"
                          src={require("../../../img/edit.png")}
                        />
                      </button>
                      <button
                        type="button"
                        className="btn btn-link "
                        onClick={() =>
                          props.action.onDelete(item.subNetworkBoundaryId)
                        }
                      >
                        <img
                          className="btnEdit"
                          src={require("../../../img/delete.png")}
                        />
                      </button>
                    </div> */}
                    </td>
                  )}
                </tr>
              ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default SubNetworkBoundaryGridPopUp;
