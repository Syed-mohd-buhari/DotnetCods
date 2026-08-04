import React, { SetStateAction, useEffect, useState, useRef } from "react";
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
  ReconciliationDtoGrid,
  ReconciliationQueryObjectGrid,
} from "../../Model/Reconciliation";
import { GetFilterColumReconciliation } from "../../Redux/Action/Reconciliation/ReconciliationGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import { useAuth } from "./../../Hook/useAuth";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { calculateBodyWidths } from "../../Utils/gridFunction";

interface Props {
  action: {
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    setIsFiltriAttivati(value: boolean): any;
    EditDetails(
      id: number | undefined,
      idDetail: number | string | undefined
    ): any;
    updatePlannedStatus(id: any, plannedFor: any): any;
  };
  data: ReconciliationDtoGrid[] | undefined;
  pagination: ReconciliationQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
}

const ReconciliationGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<ReconciliationDtoGrid[] | undefined>([]);
  const { readonly, isPermesso } = useAuth();
  const getFiltersData = (state: RootState) =>
    state.reconciliationGridReducer.filter;
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
    isFiltriAttivati,
  } = useFilterTableCrud<ReconciliationQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumReconciliation,
    props.pagination
  );

  const [dropdownStates, setDropdownStates] = useState({});

  const [dropdownPosition, setDropdownPosition] = useState<{
    left: number;
    top: number;
  }>({ left: 0, top: 0 });

  const dropdownRef = useRef<HTMLDivElement>(null);

  // Function to toggle dropdown state for a specific row
  const toggleDropdown = (
    rowId,
    reconciliationId,
    status,
    plannedActivityId,
    plannedActivityTypeFor,
    event
  ) => {
    setDropdownStates(() => ({
      rowId,
      reconciliationId,
      status,
      plannedActivityId,
      plannedActivityTypeFor,
    }));
    const left = event.clientX + window.scrollX;
    const top = event.clientY + window.scrollY;

    const windowWidth = window.innerWidth + window.scrollX;
    const windowHeight = window.innerHeight + window.scrollY;

    const maxLeft = windowWidth - 400;
    const maxTop = windowHeight - 200;
    setDropdownPosition({
      left: Math.min(left, maxLeft),
      top: Math.min(top, maxTop),
    });
  };

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (dropdownRef.current) {
        if (
          event.target.tagName === "IMG" &&
          event.target.classList.contains("dropdown_trigger")
        ) {
          return;
        }
        setDropdownStates({});
      }
    };

    // Attach the event listener when the component mounts
    document.addEventListener("click", handleClickOutside);

    // Clean up the event listener when the component unmounts
    return () => {
      document.removeEventListener("click", handleClickOutside);
    };
  }, []);

  useEffect(() => {
    setData(props.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, []);

  //UPDATE DATA
  useEffect(() => {
    setData(props?.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, [props.data]);

  useEffect(() => {
    props.action.setIsFiltriAttivati(isFiltriAttivati);
  }, [isFiltriAttivati]);

  let firstIndex, secondIndex, thirdIndex;

  let thRefs = useRef<Array<HTMLTableCellElement | null>>([
    null,
    null,
    null,
    null,
  ]); // Refs for th elements

  const thRefss = (ref, index) => {
    if (index === 0) {
      thRefs.current[0] = ref;
      calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
    } else if (index !== 0 && index <= 3) {
      thRefs.current[index] = ref;
      calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
    }
  };

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
    <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
      <div
        className="mx-0 px-0 flex-row table-container"
        style={{ position: "relative" }}
      >
        {dropdownStates["rowId"] && (
          <Dropdown
            className="d-inline mx-2"
            show={dropdownStates["rowId"] ? true : false}
            ref={dropdownRef}
            style={
              dropdownStates["rowId"]
                ? {
                    position: "absolute",
                    top: `${dropdownPosition.top - 150}px`,
                    left: `${dropdownPosition.left}px`,
                    transform: "translate(-50%, -50%)",
                    zIndex: 9999,
                  }
                : {
                    position: "absolute",
                    top: "0px",
                    left: "0px",
                    margin: "0px",
                    opacity: "0",
                  }
            }
          >
            <div
              className={`${
                dropdownStates["rowId"] ? "dropdown-menu show" : "dropdown-menu"
              }`}
            >
              <>
                {dropdownStates["status"] == "Create Planned Activity" ? (
                  <Dropdown.Item
                    onClick={() =>
                      props.action.EditDetails(
                        dropdownStates["rowId"],
                        dropdownStates["reconciliationId"]
                      )
                    }
                  >
                    Create Planned Activity
                  </Dropdown.Item>
                ) : dropdownStates["plannedActivityId"] ? (
                  <Dropdown.Item>
                    <button
                      type="button"
                      title="Update Planned Activity Status"
                      className="btn btn-link p-0"
                      style={{ color: "#333333" }}
                      onClick={() =>
                        dropdownStates["plannedActivityId"] &&
                        props?.action.updatePlannedStatus(
                          dropdownStates["plannedActivityId"],
                          dropdownStates["plannedActivityTypeFor"]
                        )
                      }
                    >
                      Update Planned Activity Status
                    </button>
                  </Dropdown.Item>
                ) : (
                  <Dropdown.Item disabled>No Action Required</Dropdown.Item>
                )}
              </>
            </div>
          </Dropdown>
        )}
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
                    undefined,
                    undefined,
                    undefined,
                    undefined,
                    undefined,
                    undefined,
                    true
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
                key={item.reconciliationId}
                onDoubleClick={(e) =>
                  toggleDropdown(
                    item.lcmId,
                    item.reconciliationId,
                    item.status,
                    item.plannedActivityId,
                    item.plannedActivityTypeFor,
                    e
                  )
                }
              >
                {props.renderGrid
                  .filter((x) => x.show)
                  .sort((a, b) => a.order - b.order)
                  .map((td, i) =>
                    SelectGridType(
                      item[td.propertyName],
                      td.propertyName,
                      td.type,
                      undefined,
                      undefined,
                      undefined,
                      undefined,
                      i,
                      thRefs,
                      thRefss
                    )
                  )}

                <td className="actions">
                  <div className="d-inline mx-2 cursor-pointer">
                    <img
                      className="dropdown_trigger"
                      src={require("../../img/options_dots.png")}
                      style={{ cursor: "pointer", padding: "10px" }}
                      onClick={(e) => {
                        toggleDropdown(
                          item.lcmId,
                          item.reconciliationId,
                          item.status,
                          item.plannedActivityId,
                          item.plannedActivityTypeFor,
                          e
                        );
                      }}
                    />
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

export default ReconciliationGrid;
