import React, { SetStateAction, useEffect, useState } from "react";
import { Dropdown } from "react-bootstrap";
import { useSelector } from "react-redux";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { QueryObjectGrid, RenderDetail } from "../../Model/Common";
import {
  SoftwareConfigurationDtoGrid,
  SoftwareConfigurationQueryObjectGrid,
} from "../../Model/SoftwareConfiguration";
import { GetFilterColumSoftwareConfiguration } from "../../Redux/Action/SoftwareConfiguration/SoftwareConfigurationGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import { useAuth } from "./../../Hook/useAuth";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { toggleState } from "../../Hook/Common";
interface Props {
  action: {
    onDelete(id: number | undefined, orphan?: boolean): any;
    EditNotDetail(id: number | undefined): any;
    EditAndDetail(
      id: number | undefined,
      idDetail: number | string | undefined
    ): any;
    Restore(id: number | undefined): any;
    onOpenFunction?(item: any): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    setIsFiltriAttivati(value: boolean): any;
  };
  data: SoftwareConfigurationDtoGrid[] | undefined;
  pagination: SoftwareConfigurationQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
}

const SoftwareConfigurationGrid: React.FC<Props> = (props) => {
  console.log("Tems SoftwareConfigurationGrid props", props);
  const [data, setData] = useState<SoftwareConfigurationDtoGrid[] | undefined>(
    []
  );
  const { readonly, isPermesso } = useAuth();
  const getFiltersData = (state: RootState) =>
    state.softwareConfigurationGridReducer.filter;
  let filterData = useSelector(getFiltersData);
  console.log("Tems filterData", filterData);
  const {
    filtriAttivi,
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
    isFiltriAttivati,
  } = useFilterTableCrud<SoftwareConfigurationQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumSoftwareConfiguration,
    props.pagination
  );

  useEffect(() => {
    setData(props.data);
  }, []);

  //UPDATE DATA
  useEffect(() => {
    setData(props?.data);
  }, [props.data]);

  useEffect(() => {
    props.action.setIsFiltriAttivati(isFiltriAttivati);
  }, [isFiltriAttivati]);

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

  const [isVisibleMajorBubble, setIsVisibleMajorBubble] = useState(0);
  const closeBubble = () => {
    setIsVisibleMajorBubble(0);
  };

  return (
    <div className="listaApparatiContainer mt-3 mx-0 col-12 p-0 justify-content-center">
      <div className="mx-0 px-0 py-3 flex-row">
        <table className="table-responsive table-thead-sticky" tabIndex={-1}>
          <thead>
            <tr className="intestazione">
              {props.renderGrid
                .filter((x) => x.show)
                .sort((a, b) => a.order - b.order)
                .map((item, i) =>
                  item.propertyName == "plannedActivity"
                    ? SelectFilterType(
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
                        undefined,
                        "Planned Activity"
                      )
                    : SelectFilterType(
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
                        isVisibleFiltriString
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
            {data?.map((item, index) => (
              <tr
                className={`dati ${
                  props.orphanColor && item.orphan ? "orphan" : null
                }`}
                key={index}
              >
                {props.renderGrid
                  .filter((x) => x.show)
                  .sort((a, b) => a.order - b.order)
                  .map((td, i) =>
                    td.propertyName == "functionAreaDescription" ? (
                      <td
                        // ref={(ref) => {
                        //     if(i === 0) (thRefs.current[0] = ref)
                        //     else if(i <= 2) (thRefs.current[i] = ref)
                        //   }}
                        className={`${
                          isVisibleMajorBubble === index + 1
                            ? "majorHardware hasModal"
                            : "majorHardware"
                        }`}
                        key={td.propertyName + td.tab + i}
                      >
                        {item?.functionAreaDescription !== undefined &&
                        item?.functionAreaDescription !== null ? (
                          <div
                            className="majorHardware"
                            onClick={() =>
                              setIsVisibleMajorBubble(
                                toggleState(index + 1, isVisibleMajorBubble)
                              )
                            }
                          >
                            {item?.functionAreaDescription !== undefined &&
                            item?.functionAreaDescription !== null
                              ? Object.keys(item?.functionAreaDescription).map(
                                  (name, index) =>
                                    index === 0 ? (
                                      <a className="" key={name} tabIndex={-1}>
                                        {item?.functionAreaDescription &&
                                          item?.functionAreaDescription}
                                      </a>
                                    ) : null
                                )
                              : "---"}
                          </div>
                        ) : (
                          "---"
                        )}
                        {isVisibleMajorBubble === index + 1 ? (
                          <div
                            className="bubbleMenuSW pl-2"
                            onMouseLeave={closeBubble}
                          >
                            <div className="triangleBubbleTop"></div>
                            <div className="col-12 row mx-0 px-2 my-2">
                              <nav className="nav flex-column">
                                {item?.functionAreaDescription !== undefined &&
                                item?.functionAreaDescription !== null ? (
                                  <pre style={{ color: "white" }}>
                                    {JSON.stringify(
                                      JSON.parse(item?.functionAreaDescription),
                                      null,
                                      2
                                    )}
                                  </pre>
                                ) : null}
                              </nav>
                            </div>
                          </div>
                        ) : null}
                      </td>
                    ) : td.propertyName === "nodeType" ? (
                      <td className=" ">{item.nodeType ?? "null"}</td>
                    ) : (
                      SelectGridType(
                        item[td.propertyName],
                        td.propertyName,
                        td.type
                      )
                    )
                  )}
                <td className="actions"></td>
                <td className="actions">
                  {!readonly && item.swConfigFunctionAreaId !== 0 && (
                    <Dropdown className="d-inline mx-2">
                      <Dropdown.Toggle id="dropdown-autoclose-true">
                        <ThreeDot />
                      </Dropdown.Toggle>

                      <Dropdown.Menu>
                        {/* {item.deleted ? (
                          <Dropdown.Item
                            onClick={() =>
                              props.action.Restore(item.networkElementAsIsId)
                            }
                          >
                            Restore
                          </Dropdown.Item>
                        ) : (
                          <>
                            <Dropdown.Item
                              onClick={() =>
                                props.action.EditNotDetail(
                                  item.networkElementAsIsId
                                )
                              }
                            >
                              Edit
                            </Dropdown.Item>
                            <Dropdown.Item
                              onClick={() =>
                                props.action.onDelete(
                                  item.networkElementAsIsId,
                                  item.orphan
                                )
                              }
                            >
                              Delete
                            </Dropdown.Item>
                          </>
                        )} */}
                        {item.swConfigFunctionAreaId !== 0 && (
                          <Dropdown.Item
                            onClick={() =>
                              props?.action.onOpenFunction &&
                              props.action.onOpenFunction(item)
                            }
                          >
                            View Sub Function Area
                          </Dropdown.Item>
                        )}
                      </Dropdown.Menu>
                    </Dropdown>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default SoftwareConfigurationGrid;
