import React, { useState, useEffect, SetStateAction } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import {
  BundleUpgradeInitiativeDtoGrid,
  BundleUpgradeInitiativeQueryObjectGrid,
} from "../../Model/BundleUpgradeIniziative";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import { GetFilterColumBundleUpgradeInitiative } from "../../Redux/Action/BundleUpgradeInitiative/BundleUpgradeInitiativeGridAction";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { RenderDetail } from "../../Model/Common";
import { Dropdown } from "react-bootstrap";
import { useAuth } from "../../Hook/useAuth";
import ThreeDot from "../../Components/TableCrud/ThreeDot";

interface Props {
  action: {
    onDelete(id: number | undefined, orphan?: boolean): any;
    Edit(id: number | undefined): any;
    Restore(id: number | undefined): any;
    Filter(
      obj: SetStateAction<BundleUpgradeInitiativeQueryObjectGrid> | undefined
    ): any;
  };
  data: BundleUpgradeInitiativeDtoGrid[] | undefined;
  pagination: BundleUpgradeInitiativeQueryObjectGrid | undefined;
  orphanColor?: boolean;

  renderGrid: RenderDetail[];
}

const BundleUpgrade: React.FC<Props> = (props) => {
  const [data, setData] = useState<
    BundleUpgradeInitiativeDtoGrid[] | undefined
  >([]);
  const getFiltersData = (state: RootState) =>
    state.bundleUpgradeInitiativeGridReducer.filter;
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
  } = useFilterTableCrud<BundleUpgradeInitiativeQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumBundleUpgradeInitiative,
    props.pagination
  );

  const { readonly, isPermesso } = useAuth();

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
    <div className="listaApparatiContainer mt-3 mx-0 col-12 p-0 justify-content-center">
      <div className="mx-0 px-0 py-3 flex-row">
        <table className="table-responsive">
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
                    false
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
                <tr
                  className={`dati ${
                    props.orphanColor && item.orphan ? "orphan" : null
                  }`}
                  key={item.bundleUpgradeInitiativeId}
                >
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
                    {!readonly && (
                      <Dropdown className="d-inline mx-2">
                        <Dropdown.Toggle id="dropdown-autoclose-true">
                          <ThreeDot />
                        </Dropdown.Toggle>

                        <Dropdown.Menu>
                          {item.deleted ? (
                            <Dropdown.Item
                              onClick={() =>
                                props.action.Restore(
                                  item.bundleUpgradeInitiativeId
                                )
                              }
                            >
                              Restore
                            </Dropdown.Item>
                          ) : (
                            <>
                              <Dropdown.Item
                                onClick={() =>
                                  props.action.Edit(
                                    item.bundleUpgradeInitiativeId
                                  )
                                }
                              >
                                Edit
                              </Dropdown.Item>
                              <Dropdown.Item
                                onClick={() =>
                                  props.action.onDelete(
                                    item.bundleUpgradeInitiativeId,
                                    item.orphan
                                  )
                                }
                              >
                                Delete
                              </Dropdown.Item>
                            </>
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

export default BundleUpgrade;
