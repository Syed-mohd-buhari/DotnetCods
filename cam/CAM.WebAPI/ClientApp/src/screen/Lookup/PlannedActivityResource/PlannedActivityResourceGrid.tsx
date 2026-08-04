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
import { GetFilterColumPlannedActivityResource } from "../../../Redux/Action/LookUp/PlannedActivityResource/PlannedActivityResourceGridAction";

import {
  PlannedActivityResourceDtoGrid,
  PlannedActivityResourceQueryObjectGrid,
} from "../../../Model/LookUp/PlannedActivityResource";
import { rtnRuleGrid, upperFirstLetter } from "../../../Hook/Common";
import { Dropdown } from "react-bootstrap";
import { useAuth } from "./../../../Hook/useAuth";
import ThreeDot from "../../../Components/TableCrud/ThreeDot";

interface Props {
  action: {
    onDelete(id: number | undefined): any;
    Edit(id: number | undefined): any;
    Filter(
      obj: SetStateAction<PlannedActivityResourceQueryObjectGrid> | undefined
    ): any;
  };
  data: PlannedActivityResourceDtoGrid[] | undefined;
  pagination: PlannedActivityResourceQueryObjectGrid | undefined;
  rules: { key: number; value: string }[];
  rulesNetworkElement: { key: number; value: string }[];
  rulesLinkedDesignComponent: { key: number; value: string }[];
  ruleActivityDetailsNetworkElement: { key: number; value: string }[];
  renderGrid: RenderDetail[];
}

const PlannedActivityResourceGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<
    PlannedActivityResourceDtoGrid[] | undefined
  >([]);
  const getFiltersData = (state: RootState) =>
    state.plannedActivityResourceGridReducer.filter;
  let filterData = useSelector(getFiltersData);
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
  } = useFilterTableCrud<PlannedActivityResourceQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumPlannedActivityResource,
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
    console.log(filtriAttivi);
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
  const { readonly, isPermesso } = useAuth();
  const thActionDate = {
    checkFilter: checkFilterDateinValue,
    settingVisibility: setIsVisibleFiltriString,
    resetFilter: resetFilterDate,
  };

  return (
    <div className="listaApparatiContainer mb-3 row mx-0 col-12 p-0 d-flex justify-content-center">
      <div className="col-12 py-3">
        <table className="w-100 table table-responsive">
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
                    item.propertyName.toLowerCase() ===
                      "plannedactivityresourcedescription"
                      ? "Planned Activity Value"
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
                <tr className="dati" key={item.plannedActivityResourceId}>
                  {props.renderGrid
                    .sort((a, b) => a.order - b.order)
                    .filter((x) => x.show)
                    .map((td, i) =>
                      td.propertyName === "ruleActicvityDetails"
                        ? SelectGridType(
                            rtnRuleGrid(item[td.propertyName], props.rules),
                            td.propertyName,
                            td.type
                          )
                        : td.propertyName === "ruleNetworkElement" ||
                          td.propertyName === "ruleAddAsset" ||
                          td.propertyName === "ruleEditAsset"
                        ? SelectGridType(
                            rtnRuleGrid(
                              item[td.propertyName],
                              props.rulesNetworkElement
                            ),
                            td.propertyName,
                            td.type
                          )
                        : td.propertyName === "ruleLinkedDc"
                        ? SelectGridType(
                            rtnRuleGrid(
                              item[td.propertyName],
                              props.rulesLinkedDesignComponent
                            ),
                            td.propertyName,
                            td.type
                          )
                        : td.propertyName ===
                            "ruleActicvityDetailsNetworkElement" ||
                          td.propertyName === "ruleActicvityDetailsAddAsset" ||
                          td.propertyName === "ruleActicvityDetailsEditAsset"
                        ? SelectGridType(
                            rtnRuleGrid(
                              item[td.propertyName],
                              props.ruleActivityDetailsNetworkElement
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
                    {!readonly && (
                      <Dropdown className="d-inline mx-2">
                        <Dropdown.Toggle id="dropdown-autoclose-true">
                          <ThreeDot />
                        </Dropdown.Toggle>

                        <Dropdown.Menu>
                          <Dropdown.Item
                            onClick={() =>
                              props.action.Edit(item.plannedActivityResourceId)
                            }
                          >
                            Edit
                          </Dropdown.Item>
                          <Dropdown.Item
                            onClick={() =>
                              props.action.onDelete(
                                item.plannedActivityResourceId
                              )
                            }
                          >
                            Delete
                          </Dropdown.Item>
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

export default PlannedActivityResourceGrid;
