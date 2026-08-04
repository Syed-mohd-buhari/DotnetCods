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
import { GetFilterColumNFVIBundleID } from "../../../Redux/Action/LookUp/NFVIBundleID/NFVIBundleIDGridAction";

import {
  NFVIBundleIDDtoGrid,
  NFVIBundleIDQueryObjectGrid,
} from "../../../Model/LookUp/NFVIBundleId";
import { Dropdown } from "react-bootstrap";

interface Props {
  action: {
    onDelete(id: number | undefined): any;
    Edit(id: number | undefined): any;
    Filter(obj: SetStateAction<NFVIBundleIDQueryObjectGrid> | undefined): any;
  };
  data: NFVIBundleIDDtoGrid[] | undefined;
  pagination: NFVIBundleIDQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
}

const NFVIBundleIDGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<NFVIBundleIDDtoGrid[] | undefined>([]);
  const getFiltersData = (state: RootState) =>
    state.nFVIBundleIDGridReducer.filter;
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
  } = useFilterTableCrud<NFVIBundleIDQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumNFVIBundleID,
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
      <div className="col-12 py-3 table-responsive">
        <table className="w-100">
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
                      ? "New NFVI Bundle ID"
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
              data
                ?.sort((a, b) => ((a.order ?? 0) < (b.order ?? 0) ? -1 : 1))
                .map((item, i) => (
                  <tr className="dati" key={item.id}>
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
                            <img
                              src={require("../../../img/options_dots.png")}
                            />
                          </Dropdown.Toggle>

                          <Dropdown.Menu>
                            <>
                              <Dropdown.Item
                                onClick={() => props.action.onDelete(item.id)}
                              >
                                Delete
                              </Dropdown.Item>
                              <Dropdown.Item
                                onClick={() => props.action.Edit(item.id)}
                              >
                                Edit
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

export default NFVIBundleIDGrid;
