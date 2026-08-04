import React, { SetStateAction, useEffect, useState, useRef } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { QueryObjectGrid, RenderDetail } from "../../Model/Common";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import { toggleState } from "../../Hook/Common";
import {
  NetworkElementAsPlannedDtoGrid,
  NetworkElementAsPlannedQueryObjectGrid,
} from "../../Model/NetworkElementAsPlanned";
import { GetFilterColumNetworkElementAsPlanned } from "../../Redux/Action/NetworkElementAsPlanned/NetworkElementAsPlannedGridAction";
import { Dropdown } from "react-bootstrap";
import { useAuth } from "./../../Hook/useAuth";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { calculateBodyWidths } from "../../Utils/gridFunction";

interface Props {
  action: {
    onDelete(id: number | undefined, orphan?: boolean): any;
    EditNotDetail(id: number | undefined): any;
    EditAndDetail(
      id: number | undefined,
      idDetail: number | string | undefined
    ): any;
    Restore(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    setIsFiltriAttivati(value: boolean): any;
    HModalDetails(
      type: string,
      id: number | undefined,
      opCoId: number | undefined
    ): any;
  };
  data: NetworkElementAsPlannedDtoGrid[] | undefined;
  pagination: NetworkElementAsPlannedQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const NetworkElementAsPlannedGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<
    NetworkElementAsPlannedDtoGrid[] | undefined
  >([]);
  const getFiltersData = (state: RootState) =>
    state.networkElementAsPlannedGridReducer.filter;
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
  } = useFilterTableCrud<NetworkElementAsPlannedQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumNetworkElementAsPlanned,
    props.pagination
  );

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

  const [dropdownStates, setDropdownStates] = useState({});

  const [dropdownPosition, setDropdownPosition] = useState<{
    left: number;
    top: number;
  }>({ left: 0, top: 0 });

  const dropdownRef = useRef<HTMLDivElement>(null);

  // Function to toggle dropdown state for a specific row
  const toggleDropdown = (
    rowId,
    plannedActivity,
    isDeleted,
    orphan,
    opCoId,
    isVirtualizedOrContanarized,
    event
  ) => {
    setDropdownStates(() => ({
      rowId,
      plannedActivity,
      isDeleted,
      orphan,
      opCoId,
      isVirtualizedOrContanarized,
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

  const [isVisibleMajorBubble, setIsVisibleMajorBubble] = useState(0);
  const { readonly, isPermesso } = useAuth();
  const closeBubble = () => {
    setIsVisibleMajorBubble(0);
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
                  <Dropdown.Item
                    disabled={
                      !(
                        dropdownStates["plannedActivity"] &&
                        Object.keys(dropdownStates["plannedActivity"]).length >
                          0
                      )
                    }
                    onClick={() => {
                      if (
                        dropdownStates["plannedActivity"] &&
                        Object.keys(dropdownStates["plannedActivity"]).length >
                          0
                      ) {
                        props.action.EditAndDetail(
                          dropdownStates["rowId"],
                          undefined
                        );
                      }
                    }}
                  >
                    Go To Planned Activities
                  </Dropdown.Item>
                  <Dropdown.Item
                    disabled={!dropdownStates["isVirtualizedOrContanarized"]}
                    onClick={() =>
                      props.action.HModalDetails(
                        "view",
                        dropdownStates["rowId"],
                        dropdownStates["opCoId"]
                      )
                    }
                  >
                    View Hardware Ancillary
                  </Dropdown.Item>
                  <Dropdown.Item
                    onClick={() =>
                      props.action.EditNotDetail(dropdownStates["rowId"])
                    }
                  >
                    Edit
                  </Dropdown.Item>
                  <Dropdown.Item
                    onClick={() =>
                      props.action.onDelete(
                        dropdownStates["rowId"],
                        dropdownStates["orphan"]
                      )
                    }
                  >
                    Delete
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
                .filter((x) => x.show)
                .sort((a, b) => a.order - b.order)
                .map((item, i) =>
                  item.propertyName == "plannedActivity"
                    ? SelectFilterType(
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
                        "Planned Activity",
                        undefined,
                        undefined,
                        true
                      )
                    : SelectFilterType(
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
                key={item.networkElementAsPlannedId}
                onDoubleClick={(e) =>
                  toggleDropdown(
                    item.networkElementAsPlannedId,
                    item.plannedActivity,
                    item.deleted,
                    item.orphan,
                    item.opCoId,
                    item.isVirtualizedOrContanarized,
                    e
                  )
                }
              >
                {props.renderGrid
                  .filter((x) => x.show)
                  .sort((a, b) => a.order - b.order)
                  .map((td, i) =>
                    td.propertyName === "plannedActivity" ? (
                      <td
                        className="majorHardware"
                        key={`${i}${item.networkElementAsPlannedId}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <div
                          className="majorHardware"
                          onClick={() =>
                            setIsVisibleMajorBubble(
                              toggleState(index + 1, isVisibleMajorBubble)
                            )
                          }
                        >
                          {item?.plannedActivity !== undefined
                            ? Object.keys(item?.plannedActivity).map(
                                (name, index) =>
                                  index === 0 ? (
                                    <a className="" key={name} tabIndex={-1}>
                                      {item?.plannedActivity &&
                                        item?.plannedActivity[name]}
                                    </a>
                                  ) : null
                              )
                            : "---"}
                        </div>
                        {isVisibleMajorBubble === index + 1 ? (
                          <div
                            className="bubbleMenu"
                            onMouseLeave={closeBubble}
                          >
                            <div className="triangleBubbleTop"></div>
                            <div className="col-12 row mx-0 px-2 my-2">
                              <nav className="nav flex-column">
                                {item?.plannedActivity != undefined
                                  ? Object.keys(item?.plannedActivity).map(
                                      (name) => (
                                        <a
                                          onClick={() =>
                                            props.action.EditAndDetail(
                                              item.networkElementAsPlannedId,
                                              name
                                            )
                                          }
                                          key={name}
                                          className="text-white fakeLink"
                                        >
                                          {item?.plannedActivity &&
                                            item?.plannedActivity[name]}
                                        </a>
                                      )
                                    )
                                  : null}
                              </nav>
                            </div>
                          </div>
                        ) : null}
                      </td>
                    ) : (
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
                            item.networkElementAsPlannedId,
                            item.plannedActivity,
                            item.deleted,
                            item.orphan,
                            item.opCoId,
                            item.isVirtualizedOrContanarized,
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

export default NetworkElementAsPlannedGrid;
