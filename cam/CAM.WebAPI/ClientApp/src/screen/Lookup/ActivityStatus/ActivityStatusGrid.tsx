import React, { useState, useEffect, SetStateAction } from "react";
import "../../../Css/App.css";
import "../../../Css/index.css";
import "../../../Css/NetworkElement.css";
import "../../../Css/Toggle.css";
import {
  SelectFilterType,
  SelectGridType,
} from "../../../Hook/CommonRenderGrid/GridRender";
import { useSelector } from "react-redux";
import { RootState } from "../../../Redux/Store/rootStore";
import { useFilterTableCrud } from "../../../Hook/useFilterTableCrud";
import { RenderDetail } from "../../../Model/Common";
import { GetFilterColumActivityStatus } from "../../../Redux/Action/LookUp/ActivityStatus/ActivityStatusGridAction";
import {
  TipologicheQueryObjectGridCombinationRule,
  TipologicaGridDtoCombinationRule,
} from "../../../Model/LookUp/LookUpGenericModel";
import { rtnRuleGrid } from "../../../Hook/Common";
import { Dropdown } from "react-bootstrap";
import ThreeDot from "../../../Components/TableCrud/ThreeDot";

interface Props {
  action: {
    onDelete(id: number | undefined): any;
    Edit(id: number | undefined): any;
    Filter(
      obj: SetStateAction<TipologicheQueryObjectGridCombinationRule> | undefined
    ): any;
  };
  data: TipologicaGridDtoCombinationRule[] | undefined;
  pagination: TipologicheQueryObjectGridCombinationRule | undefined;

  renderGrid: RenderDetail[];
  rulesProjectStatus: { key: number; value: string }[];
  rules: { key: number; value: string }[];
}

const ActivityStatusGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<
    TipologicaGridDtoCombinationRule[] | undefined
  >([]);
  const getFiltersData = (state: RootState) =>
    state.activityStatusGridReducer.filter;
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
  } = useFilterTableCrud<TipologicheQueryObjectGridCombinationRule>(
    props.action.Filter,
    GetFilterColumActivityStatus,
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
                    undefined,
                    item.propertyName.toLowerCase() === "description"
                      ? "Activity Status"
                      : undefined
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
                <tr className="dati" key={item.id}>
                  {props.renderGrid
                    .sort((a, b) => a.order - b.order)
                    .filter((x) => x.show)
                    .map((td, i) =>
                      td.propertyName === "rule"
                        ? SelectGridType(
                            rtnRuleGrid(item[td.propertyName], props.rules),
                            td.propertyName,
                            td.type
                          )
                        : td.propertyName === "projectStatusCombinationRule"
                        ? SelectGridType(
                            rtnRuleGrid(
                              item[td.propertyName] ?? 0,
                              props.rulesProjectStatus
                            ),
                            td.propertyName,
                            td.type
                          )
                        : SelectGridType(
                            item[td.propertyName],
                            td.propertyName,
                            td.type
                          )
                    )}
                  <td className="actions">
                    <Dropdown className="d-inline mx-2">
                      <Dropdown.Toggle id="dropdown-autoclose-true">
                        <ThreeDot />
                      </Dropdown.Toggle>
                      <Dropdown.Menu>
                        {
                          <>
                            <Dropdown.Item
                              onClick={() => props.action.Edit(item.id)}
                            >
                              Edit
                            </Dropdown.Item>
                            <Dropdown.Item
                              onClick={() => props.action.onDelete(item.id)}
                            >
                              Delete
                            </Dropdown.Item>
                          </>
                        }
                      </Dropdown.Menu>
                    </Dropdown>
                  </td>
                </tr>
              ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default ActivityStatusGrid;
