import React, { SetStateAction, useEffect, useState } from "react";
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
import { useAuth } from "./../../Hook/useAuth";
import { RootState } from "../../Redux/Store/rootStore";
import { useSelector } from "react-redux";
import { toggleState } from "../../Hook/Common";

interface Props {
  action: {
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    setIsFiltriAttivati(value: boolean): any;
  };
  data: SoftwareConfigurationDtoGrid[] | undefined;
  pagination: SoftwareConfigurationQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
}

const SoftwareConfigurationGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<SoftwareConfigurationDtoGrid[] | undefined>(
    []
  );
  const { readonly, isPermesso } = useAuth();
  const getFiltersData = (state: RootState) =>
    state.subFunctionFilterGridReducer.filter;
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
    undefined,
    props.pagination
  );
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
                    false,
                    undefined,
                    true
                  )
                )}
            </tr>
          </thead>
          <tbody>
            {data?.map((item, index) => (
              <tr className={`dati`} key={index}>
                {props.renderGrid
                  .filter((x) => x.show)
                  .sort((a, b) => a.order - b.order)
                  .map((td, i) =>
                    td.propertyName == "subFuncAreaDescription" ? (
                      <td
                        className={`${
                          isVisibleMajorBubble === index + 1
                            ? "majorHardware hasModal"
                            : "majorHardware"
                        }`}
                        key={td.propertyName + td.tab + i}
                        style={{ minWidth: "330px" }}
                      >
                        {item["subFuncAreaDescription"] !== undefined &&
                        item["subFuncAreaDescription"] !== null ? (
                          <div
                            className="majorHardware"
                            onClick={() =>
                              setIsVisibleMajorBubble(
                                toggleState(index + 1, isVisibleMajorBubble)
                              )
                            }
                          >
                            {item["subFuncAreaDescription"] !== undefined &&
                            item["subFuncAreaDescription"] !== null
                              ? Object.keys(item["subFuncAreaDescription"]).map(
                                  (name, index) =>
                                    index === 0 ? (
                                      <a className="" key={name} tabIndex={-1}>
                                        {item["subFuncAreaDescription"] &&
                                          item["subFuncAreaDescription"]}
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
                                {item["subFuncAreaDescription"] !== undefined &&
                                item["subFuncAreaDescription"] !== null ? (
                                  <pre style={{ color: "white" }}>
                                    {JSON.stringify(
                                      JSON.parse(
                                        item["subFuncAreaDescription"]
                                      ),
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
                    ) : (
                      SelectGridType(
                        item[td.propertyName],
                        td.propertyName,
                        td.type
                      )
                    )
                  )}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default SoftwareConfigurationGrid;
