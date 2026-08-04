import React, { SetStateAction, useEffect, useRef, useState } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";

import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import {
  LcmEngineeringDtoGrid,
  LcmEngineringQueryObjectGrid,
} from "../../Model/LcmEngineering";
import { GetFilterColumLcmEngineering } from "../../Redux/Action/LcmEngineering/LcmEngineeringGridAction";
import { QueryObjectGrid, RenderDetail } from "../../Model/Common";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
// import TH from "../../Components/TableCrud/TableCrudTH";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";

interface Props {
  action: {
    Delete(id: number | undefined, orphan?: boolean): any;
    Edit(id: number | undefined): any;
    Restore(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
  };
  data: LcmEngineeringDtoGrid[] | undefined;
  pagination: LcmEngineringQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
}

const LcmEngineeringPlannedActivities: React.FC<Props> = (props) => {
  const [data, setData] = useState<LcmEngineeringDtoGrid[] | undefined>([]);
  const getFiltersData = (state: RootState) =>
    state.lcmEngineeringGridReducer.filter;
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
  } = useFilterTableCrud<LcmEngineringQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumLcmEngineering,
    props.pagination
  );
  useEffect(() => {
    setData(props.data);
  }, []);
  //UPDATE DATA
  useEffect(() => {
    setData(props?.data);
  }, [props.data]);

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
    <div className="listaApparatiContainer mt-3 row mx-0 col-12 p-0 d-flex justify-content-center">
      <div className="col-12 mx-0 px-0">
        <table className="table table-borderless  ">
          <thead>
            <tr className="intestazione">
              {props.renderGrid
                .filter((x) => x.show && x.tab === "plannedActivities")
                .sort((a, b) => a.order - b.order)
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
                key={item.lcmEngineeringId}
              >
                {props.renderGrid
                  .filter((x) => x.show && x.tab === "plannedActivities")
                  .sort((a, b) => a.order - b.order)
                  .map((td, i) =>
                    SelectGridType(
                      item[td.propertyName],
                      td.propertyName,
                      td.type
                    )
                  )}
                <td className=" ">
                  {item.deleted ? (
                    <div className="d-flex flex-row">
                      <button
                        type="button"
                        className="btn btn-link"
                        onClick={() =>
                          props.action.Restore(item.lcmEngineeringId)
                        }
                      >
                        <img
                          className="btnEdit"
                          src={require("../../img/restore.png")}
                        />
                      </button>
                    </div>
                  ) : (
                    <div className="d-flex flex-row">
                      <button
                        type="button"
                        className="btn btn-link"
                        onClick={() => props.action.Edit(item.lcmEngineeringId)}
                      >
                        <img
                          className="btnEdit"
                          src={require("../../img/edit.png")}
                        />
                      </button>
                      <button
                        type="button"
                        className="btn btn-link"
                        onClick={() =>
                          props.action.Delete(
                            item.lcmEngineeringId,
                            item.orphan
                          )
                        }
                      >
                        <img
                          className="btnEdit"
                          src={require("../../img/delete.png")}
                        />
                      </button>
                    </div>
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

export default LcmEngineeringPlannedActivities;
