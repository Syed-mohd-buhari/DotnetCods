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
import { GetFilterColumDeploymentStatus } from "../../../Redux/Action/LookUp/DeploymentStatus/DeploymentStatusGridAction";
import {
  TipologicheQueryObjectGridRule,
  TipologicaGridDtoRule,
} from "../../../Model/LookUp/LookUpGenericModel";
import { rtnRuleGrid } from "../../../Hook/Common";
import {
  DeploymentStatusDtoGrid,
  DeploymentStatusQuery,
} from "../../../Model/LookUp/DeploymentStatus";
import { Dropdown } from "react-bootstrap";
import ThreeDot from "../../../Components/TableCrud/ThreeDot";

interface Props {
  action: {
    onDelete(id: number | undefined): any;
    Edit(id: number | undefined): any;
    Filter(obj: SetStateAction<DeploymentStatusQuery> | undefined): any;
  };
  data: DeploymentStatusDtoGrid[] | undefined;
  pagination: DeploymentStatusQuery | undefined;
  rules: { key: number; value: string }[];
  renderGrid: RenderDetail[];
}

const DeploymentStatusGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<DeploymentStatusDtoGrid[] | undefined>([]);
  const getFiltersData = (state: RootState) =>
    state.deploymentStatusGridReducer.filter;
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
  } = useFilterTableCrud<DeploymentStatusQuery>(
    props.action.Filter,
    GetFilterColumDeploymentStatus,
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
    <div className="listaApparatiContainer row mx-0 col-12 p-0 d-flex justify-content-center">
      <div className="col-12 mx-0 px-0 py-3">
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
                    item.propertyName.toLowerCase() ===
                      "deploymentStatusDescription"
                      ? "Asset Deployment Status"
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
                <tr className="dati" key={item.deploymentStatusId}>
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
                              onClick={() =>
                                props.action.Edit(item.deploymentStatusId)
                              }
                            >
                              Edit
                            </Dropdown.Item>
                            <Dropdown.Item
                              onClick={() =>
                                props.action.onDelete(item.deploymentStatusId)
                              }
                            >
                              Delete
                            </Dropdown.Item>
                          </>
                        }
                      </Dropdown.Menu>
                    </Dropdown>
                    {/* <div className="d-flex flex-row justify-content-end">
                      <button
                        type="button"
                        className="btn btn-link "
                        onClick={() =>
                          props.action.Edit(item.deploymentStatusId)
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
                          props.action.onDelete(item.deploymentStatusId)
                        }
                      >
                        <img
                          className="btnEdit"
                          src={require("../../../img/delete.png")}
                        />
                      </button>
                    </div> */}
                  </td>
                </tr>
              ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default DeploymentStatusGrid;
