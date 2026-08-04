import React, { SetStateAction, useEffect, useState, useRef } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { BuildBagDtoGrid, BuildBagQueryObjectGrid } from "../../Model/BuildBag";
import { GetFilterColumBuildBag } from "../../Redux/Action/BuildBag/BuildBagGridAction";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { QueryObjectGrid, RenderDetail } from "../../Model/Common";
import TH from "../../Components/TableCrud/TableCrudTH";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";

import { Dropdown } from "react-bootstrap";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { calculateBodyWidths } from "../../Utils/gridFunction";
import { toggleState } from "../../Hook/Common";

interface Props {
  action: {
    onDelete(id: number | undefined, orphan?: boolean): any;
    Edit(id: number | undefined): any;
    View(id: number | undefined): any;
    Restore(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    setIsFiltriAttivati(value: boolean): any;
    getSoftwareComponentUpgradeData(value: number): any;
  };
  data: BuildBagDtoGrid[] | undefined;
  pagination: BuildBagQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
  readonly?: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const ComponentSwGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<BuildBagDtoGrid[] | undefined>([]);
  const getFiltersData = (state: RootState) => state.buildBagGridReducer.filter;
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
  } = useFilterTableCrud<BuildBagQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumBuildBag,
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
  const toggleDropdown = (rowId, isDeleted, orphan, event, isLcmAssociated) => {
    setDropdownStates(() => ({
      rowId,
      isDeleted,
      orphan,
      isLcmAssociated,
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
  const [isVisibleBubble, setIsVisibleBubble] = useState(0);
  const closeBubble = () => {
    setIsVisibleBubble(0);
  };
  return (
    <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
      <div
        className="mx-0 px-0 flex-row table-container"
        style={{ position: "relative" }}
      >
        {!props.readonly && dropdownStates["rowId"] && (
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
                    onClick={() =>
                      props.action.getSoftwareComponentUpgradeData(
                        dropdownStates["rowId"] ?? 0
                      )
                    }
                  >
                    Upgrade Bag Item
                  </Dropdown.Item>
                  <Dropdown.Item
                    onClick={() => props.action.View(dropdownStates["rowId"])}
                  >
                    View Component
                  </Dropdown.Item>
                  <Dropdown.Item
                    onClick={() => props.action.Edit(dropdownStates["rowId"])}
                    // disabled={
                    //   dropdownStates["isLcmAssociated"] === "Yes" ? true : false
                    // }
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
                    disabled={
                      dropdownStates["isLcmAssociated"] === "Yes" ? true : false
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
            {data?.map((item, index) => (
              <tr
                className={`dati ${
                  props.orphanColor && item.orphan ? "orphan" : null
                }`}
                key={item.buildBagId}
                onDoubleClick={(e) =>
                  toggleDropdown(
                    item.buildBagId,
                    item.deleted,
                    item.orphan,
                    e,
                    item.associatedWithLcm
                  )
                }
              >
                {props.renderGrid
                  .sort((a, b) => a.order - b.order)
                  .filter((x) => x.show)
                  .map((td, i) =>
                    td.propertyName == "mappedComponentSoftwareBuild" ? (
                      <td
                        // ref={(ref) => {
                        //     if(i === 0) (thRefs.current[0] = ref)
                        //     else if(i <= 2) (thRefs.current[i] = ref)
                        //   }}
                        className={`${
                          isVisibleBubble === index + 1
                            ? "majorHardware hasModal"
                            : "majorHardware"
                        }`}
                        key={td.propertyName + td.tab + i}
                      >
                        {item?.mappedComponentSoftwareBuild !== undefined &&
                        item?.mappedComponentSoftwareBuild !== null ? (
                          <div
                            className="majorHardware"
                            onClick={() =>
                              setIsVisibleBubble(
                                toggleState(index + 1, isVisibleBubble)
                              )
                            }
                          >
                            {item?.mappedComponentSoftwareBuild !== undefined &&
                            item?.mappedComponentSoftwareBuild !== null
                              ? Object.keys(
                                  item?.mappedComponentSoftwareBuild
                                ).map((name, index) =>
                                  index === 0 ? (
                                    <a className="" key={name} tabIndex={-1}>
                                      {item?.mappedComponentSoftwareBuild &&
                                        item?.mappedComponentSoftwareBuild}
                                    </a>
                                  ) : null
                                )
                              : "---"}
                          </div>
                        ) : (
                          "---"
                        )}
                        {isVisibleBubble === index + 1 ? (
                          <div
                            className="bubbleMenuSW pl-2"
                            onMouseLeave={closeBubble}
                          >
                            <div className="triangleBubbleTop"></div>
                            <div className="col-12 row mx-0 px-2 my-2">
                              <nav className="nav flex-column">
                                {item?.mappedComponentSoftwareBuild !==
                                  undefined &&
                                item?.mappedComponentSoftwareBuild !== null ? (
                                  <pre style={{ color: "white" }}>
                                    {item?.mappedComponentSoftwareBuild
                                      ?.split(/[\n,]+/) // Split by newline and comma
                                      .filter((line) => line.trim() !== "") // Remove empty lines
                                      .map((line, index) => (
                                        <div key={index}>{line.trim()}</div> // Render each line separately
                                      ))}
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
                        td.type,
                        "",
                        undefined,
                        item["eomStatus"],
                        "eomStatus",
                        i,
                        thRefs,
                        thRefss
                      )
                    )
                  )}
                <td className="actions">
                  {!props.readonly && (
                    <div className="d-inline mx-2 cursor-pointer">
                      <img
                        className="dropdown_trigger"
                        src={require("../../img/options_dots.png")}
                        style={{ cursor: "pointer", padding: "10px" }}
                        onClick={(e) => {
                          toggleDropdown(
                            item.buildBagId,
                            item.deleted,
                            item.orphan,
                            e,
                            item.associatedWithLcm
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
export default ComponentSwGrid;
