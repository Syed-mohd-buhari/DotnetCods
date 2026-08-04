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
import { GetFilterColumMajorHardwareMT } from "../../../Redux/Action/LookUp/MajorHardwareMT/MajorHardwareMTGridAction";
import {
  TipologicheQueryObjectGrid,
  TipologicaGridDto,
} from "../../../Model/LookUp/LookUpGenericModel";
import {
  MajorHardwareMTDtoGrid,
  MajorHardwareMTQueryObjectGrid,
} from "../../../Model/LookUp/MajorHardwareMT";
import { Dropdown } from "react-bootstrap";
import ThreeDot from "../../../Components/TableCrud/ThreeDot";

interface Props {
  action: {
    Delete(id: number | undefined): any;
    Edit(id: number | undefined): any;
    Filter(
      obj: SetStateAction<MajorHardwareMTQueryObjectGrid> | undefined
    ): any;
  };
  data: MajorHardwareMTDtoGrid[] | undefined;
  pagination: MajorHardwareMTQueryObjectGrid | undefined;

  renderGrid: RenderDetail[];
}

const MajorHardwareMTGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<MajorHardwareMTDtoGrid[] | undefined>([]);
  const getFiltersData = (state: RootState) =>
    state.majorHardwareMTGridReducer.filter;
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
  } = useFilterTableCrud<MajorHardwareMTQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumMajorHardwareMT,
    props.pagination
  );

  //CARICAMENTO INIZIALE
  useEffect(() => {
    setData(props.data);
  }, []);
  //UPDATE DATA
  useEffect(() => {
    setData(props?.data);
  }, [props.data]);
  useEffect(() => {}, [props.renderGrid, props.data]);
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

  return (
    <div className="listaApparatiContainer mt-3 row mx-0 col-12 p-0">
      <div className="col-12 py-3" style={{ overflowX: "auto", width: "100%" }}>
        <table
          className=""
          style={{ width: "100%", minWidth: "900px", tableLayout: "auto" }}
        >
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
              data?.map((item, i) => (
                <tr className="dati" key={item.majorHardwareBuildAsisId}>
                  {props.renderGrid
                    .sort((a, b) => a.order - b.order)
                    .filter((x) => x.show)
                    .map((td, i) =>
                      SelectGridType(
                        item[td.propertyName],
                        td.propertyName,
                        td.type
                      )
                    )}
                  <td className="actions">
                    <div className="d-flex flex-row justify-content-end">
                      <Dropdown className="d-inline mx-2">
                        <Dropdown.Toggle id="dropdown-autoclose-true">
                          <ThreeDot />
                        </Dropdown.Toggle>

                        <Dropdown.Menu>
                          <>
                            <Dropdown.Item
                              onClick={() =>
                                props.action.Edit(item.majorHardwareBuildAsisId)
                              }
                            >
                              Edit
                            </Dropdown.Item>
                            <Dropdown.Item
                              onClick={() =>
                                props.action.Delete(
                                  item.majorHardwareBuildAsisId
                                )
                              }
                            >
                              Delete
                            </Dropdown.Item>
                          </>
                        </Dropdown.Menu>
                      </Dropdown>
                    </div>
                  </td>
                </tr>
              ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default MajorHardwareMTGrid;
