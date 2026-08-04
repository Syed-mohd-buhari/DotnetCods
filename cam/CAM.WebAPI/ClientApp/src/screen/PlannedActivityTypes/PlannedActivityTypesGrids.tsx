import React, { useState, useEffect, SetStateAction, useRef } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { RenderDetail } from "../../Model/Common";
import {
  PlannedActivityTypesDtoGrid,
  PlannedActivityTypesQueryObjectGrid,
} from "../../Model/PlannedActivityTypes";
import { Dropdown } from "react-bootstrap";
import { useAuth } from "../../Hook/useAuth";
import { GetFilterColumPlannedActivityTypes } from "../../Redux/Action/PlannedActivityTypes/PlannedActivityTypesGridAction";
import {
  PlannedActivityDtoCreate,
  PlannedActivityDtoUpdate,
} from "../../Model/PlannedActivity";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { calculateBodyWidths } from "../../Utils/gridFunction";

interface Props {
  action: {
    Delete(id: number | undefined): any;
    onEdit(item: PlannedActivityDtoCreate | undefined): any;
    Restore(id: number | undefined): any;
    Enable(id: number | undefined, linkedDc: boolean | undefined): any;
    Filter(
      obj: SetStateAction<PlannedActivityTypesQueryObjectGrid> | undefined
    ): any;
  };
  data: PlannedActivityTypesDtoGrid[] | undefined;
  pagination: PlannedActivityTypesQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const PlannedActivityTypesGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<PlannedActivityTypesDtoGrid[] | undefined>(
    []
  );
  const getFiltersData = (state: RootState) =>
    state.PlannedActivityTypesGridReducer.filter;
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
    isFiltriAttivati,
  } = useFilterTableCrud<PlannedActivityTypesQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumPlannedActivityTypes,
    props.pagination
  );

  const { readonly, isPermesso } = useAuth();

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

  //UPDATE DATA
  useEffect(() => {
    setData(props?.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, [props.data]);

  //CHIAMATA AL PARENT AL CAMBIO FILTRI
  useEffect(() => {
    props.action.Filter(filtriAttivi);
  }, [filtriAttivi]);

  const [dropdownStates, setDropdownStates] = useState({});

  const [dropdownPosition, setDropdownPosition] = useState<{
    left: number;
    top: number;
  }>({ left: 0, top: 0 });

  const dropdownRef = useRef<HTMLDivElement>(null);

  // Function to toggle dropdown state for a specific row
  const toggleDropdown = (
    rowId,
    isDeleted,
    item,
    orphan,
    linkedDcRule,
    event
  ) => {
    setDropdownStates(() => ({
      rowId,
      isDeleted,
      item,
      orphan,
      linkedDcRule,
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
    <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
      <div
        className="mx-0 px-0 flex-row table-container"
        style={{ position: "relative" }}
      >
        {!readonly && dropdownStates["rowId"] && (
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
              {dropdownStates["isDeleted"] ? (
                <Dropdown.Item
                  onClick={() => props.action.Restore(dropdownStates["rowId"])}
                >
                  Restore
                </Dropdown.Item>
              ) : (
                <>
                  {/* <Dropdown.Item
                                onClick={() =>
                                  props.action.onDelete(
                                    dropdownStates["rowId"],
                                    dropdownStates["orphan"]
                                  )
                                }
                              >
                                Delete
                              </Dropdown.Item> */}
                  <Dropdown.Item
                    onClick={() => props.action.onEdit(dropdownStates["item"])}
                  >
                    Edit
                  </Dropdown.Item>
                  <Dropdown.Item
                    onClick={() =>
                      props.action.Enable(
                        dropdownStates["rowId"],
                        dropdownStates["linkedDcRule"]
                      )
                    }
                  >
                    {dropdownStates["linkedDcRule"]
                      ? "Disable Rule"
                      : "Enable Rule"}
                  </Dropdown.Item>
                </>
              )}
            </div>
          </Dropdown>
        )}
        <table className="table-responsive table-thead-sticky" tabIndex={-1}>
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
            {data &&
              data?.map((item, i) => (
                <tr
                  className={`dati ${
                    props.orphanColor && item.orphan ? "orphan" : null
                  }`}
                  key={item.plannedActivityTypesId}
                  onDoubleClick={(e) =>
                    toggleDropdown(
                      item.plannedActivityTypesId,
                      item.deleted,
                      item,
                      item.orphan,
                      item.linkedDcRule,
                      e
                    )
                  }
                >
                  {props.renderGrid
                    .sort((a, b) => a.order - b.order)
                    .filter((x) => x.show)
                    .map((td, i) =>
                      SelectGridType(
                        item[td.propertyName],
                        td.propertyName,
                        td.type,
                        "",
                        undefined,
                        undefined,
                        undefined,
                        i,
                        thRefs,
                        thRefss
                      )
                    )}
                  <td className="actions">
                    {!readonly && (
                      <div className="d-inline mx-2 cursor-pointer">
                        <img
                          className="dropdown_trigger"
                          src={require("../../img/options_dots.png")}
                          style={{ cursor: "pointer", padding: "10px" }}
                          onClick={(e) => {
                            toggleDropdown(
                              item.plannedActivityTypesId,
                              item.deleted,
                              item,
                              item.orphan,
                              item.linkedDcRule,
                              e
                            );
                          }}
                        />
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

export default PlannedActivityTypesGrid;
