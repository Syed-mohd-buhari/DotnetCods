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
import { GetFilterColumSystemName } from "../../../Redux/Action/LookUp/Domain/DomainGridAction";
import {
  SystemNameQuery,
  SystemNamesDtoGrid,
} from "../../../Model/LookUp/Domain";
import { Dropdown } from "react-bootstrap";
import ThreeDot from "../../../Components/TableCrud/ThreeDot";

interface Props {
  action: {
    onDelete(id: number | undefined): any;
    Edit(id: number | undefined): any;
    Filter(obj: SetStateAction<SystemNameQuery> | undefined): any;
  };
  data: SystemNamesDtoGrid[] | undefined;
  pagination: SystemNameQuery | undefined;
  renderGrid: RenderDetail[];
}

const SystemNamesGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<SystemNamesDtoGrid[] | undefined>([]);
  const getFiltersData = (state: RootState) => state.systemGridReducer.filter;
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
  } = useFilterTableCrud<SystemNameQuery>(
    props.action.Filter,
    GetFilterColumSystemName,
    props.pagination
  );

  // Initial load and updates
  useEffect(() => {
    setData(props.data);
  }, [props.data]);

  // Notify parent of filter changes
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
      <div style={{ position: "relative" }}>
        <table className="w-100 table-responsive">
          <thead>
            <tr className="intestazione">
              {props.renderGrid
                .sort((a, b) => a.order - b.order)
                .filter((x) => x.show)
                .map((item) =>
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
              data.map((item) => (
                <tr className="dati" key={item.systemNameId}>
                  {props.renderGrid
                    .sort((a, b) => a.order - b.order)
                    .filter((x) => x.show)
                    .map((td) =>
                      SelectGridType(
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
                        <Dropdown.Item
                          onClick={() => props.action.Edit(item.systemNameId)}
                        >
                          Edit
                        </Dropdown.Item>
                        <Dropdown.Item
                          onClick={() =>
                            props.action.onDelete(item.systemNameId)
                          }
                        >
                          Delete
                        </Dropdown.Item>
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

export default SystemNamesGrid;
