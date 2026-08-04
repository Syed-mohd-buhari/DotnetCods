import React, { SetStateAction, useEffect, useRef, useState } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import {
  LcmEngineeringDtoGrid,
  LcmEngineringQueryObjectGrid,
} from "../../Model/LcmEngineering";
import { createSelector } from "@reduxjs/toolkit";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { QueryObjectGrid, RenderDetail } from "../../Model/Common";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import { toggleState } from "../../Hook/Common";
import { Dropdown } from "react-bootstrap";
import { useAuth } from "./../../Hook/useAuth";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { calculateBodyWidths } from "../../Utils/gridFunction";
import OpCo from "../../Containers/Lookup/OpCoContainer";
import GridLink from "../GenerateLcmDb/GridLink";

interface Props {
  action: {
    onDelete(id: number | undefined, orphan?: boolean): any;
    EditNotDetail(id: number | undefined): any;
    EditAndDetail(
      id: number | undefined,
      idDetail: number | string | undefined,
      disabledForm?: boolean,
      type?: string
    ): any;
    AuditModal(
      type: string,
      id: number | undefined,
      opCo: string | undefined
    ): any;
    Restore(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    setIsFiltriAttivati(value: boolean): any;
  };
  data: LcmEngineeringDtoGrid[] | undefined;
  pagination: LcmEngineringQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
  archivedMode?: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const LcmEngineeringOperational: React.FC<Props> = (props) => {
  let thRefs = useRef<Array<HTMLTableCellElement | null>>([
    null,
    null,
    null,
    null,
  ]); // Refs for th elements
  const [data, setData] = useState<LcmEngineeringDtoGrid[] | undefined>([]);
  const filterData = useSelector(
    (state: RootState) => state.lcmEngineeringGridReducer.filter
  );
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
  } = useFilterTableCrud<LcmEngineringQueryObjectGrid>(
    props.action.Filter,
    undefined,
    props.pagination
  );

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

  // dropdown trigger
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
    orphan,
    isLcmAncillaryData,
    event,
    opCo
  ) => {
    setDropdownStates(() => ({
      rowId,
      plannedActivity,
      orphan,
      isLcmAncillaryData,
      opCo,
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
    document.body.addEventListener("click", handleClickOutside);

    // Clean up the event listener when the component unmounts
    return () => {
      document.body.removeEventListener("click", handleClickOutside);
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
  const [isVisibleMajorOriginalBubble, setIsVisibleMajorOriginalBubble] =
    useState(0);
  const closeBubble = () => {
    setIsVisibleMajorBubble(0);
    setIsVisibleMajorOriginalBubble(0);
  };
  const { readonly, isPermesso } = useAuth();

  const thRefss = (ref, index) => {
    if (index === 0) {
      thRefs.current[0] = ref;
      calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
    } else if (index !== 0 && index <= 3) {
      thRefs.current[index] = ref;
      calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
    }
  };

  return (
    <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
      <div
        className="mx-0 px-0 flex-row table-container"
        style={{ position: "relative" }}
      >
        {!readonly && !props.archivedMode && dropdownStates["rowId"] && (
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
                <Dropdown.Item
                  disabled={
                    !(
                      dropdownStates["plannedActivity"] &&
                      Object.keys(dropdownStates["plannedActivity"]).length > 0
                    )
                  }
                  onClick={() => {
                    if (
                      dropdownStates["plannedActivity"] &&
                      Object.keys(dropdownStates["plannedActivity"]).length > 0
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
                {dropdownStates["isLcmAncillaryData"] ? (
                  <Dropdown.Item
                    onClick={() =>
                      props.action.AuditModal(
                        "view",
                        dropdownStates["rowId"],
                        dropdownStates["opCo"]
                      )
                    }
                  >
                    View Ancillary Data
                  </Dropdown.Item>
                ) : (
                  <Dropdown.Item
                    onClick={() =>
                      props.action.AuditModal(
                        "add",
                        dropdownStates["rowId"],
                        dropdownStates["opCo"]
                      )
                    }
                  >
                    Add Ancillary Data
                  </Dropdown.Item>
                )}

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
            </div>
          </Dropdown>
        )}
        <table className="table-responsive table-thead-sticky" tabIndex={-1}>
          <thead>
            <tr className="intestazione">
              {props.renderGrid
                .filter((x) => x.show)
                .sort((a, b) => a.order - b.order)
                .map((item, index) =>
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
                    item.colorHeader,
                    undefined,
                    item.propertyName === "lcmEngineeringId"
                      ? "Index"
                      : undefined,
                    undefined,
                    index,
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
                key={item.lcmEngineeringId}
                onDoubleClick={(e) => {
                  toggleDropdown(
                    item.lcmEngineeringId,
                    item.plannedActivity,
                    item.orphan,
                    item.isLcmAncillaryData,
                    e,
                    item.opCo
                  );
                }}
              >
                {props.renderGrid
                  .filter((x) => x.show)
                  .sort((a, b) => a.order - b.order)
                  .map((td, i) =>
                    td.propertyName === "buildBagDescription" ? (
                      <td
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                        className={`${
                          isVisibleMajorBubble === index + 1
                            ? "majorHardware hasModal"
                            : "majorHardware"
                        }`}
                        key={td.propertyName + td.tab + i}
                      >
                        <GridLink
                          to={{
                            pathname: "componentswbag",
                            search: "id=" + item.buildBagId,
                            state: {
                              id: item.buildBagId,
                              lcmId:
                                item.lcmEngineeringId &&
                                item?.buildBagDescription?.includes("Empty")
                                  ? item.lcmEngineeringId
                                  : null,
                              dcfId:
                                item.designComponentFamilyId &&
                                item?.buildBagDescription?.includes("Empty")
                                  ? item.designComponentFamilyId
                                  : null,
                              opCoId:
                                item.opCoId &&
                                item?.buildBagDescription?.includes("Empty")
                                  ? item.opCoId
                                  : null,
                              tab: "",
                              prevPage: "lcmengineering",
                            },
                          }}
                          title={item.buildBagDescription}
                        />
                      </td>
                    ) : td.propertyName === "plannedActivity" ? (
                      <td
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                        className={`${
                          isVisibleMajorBubble === index + 1
                            ? "majorHardware hasModal"
                            : "majorHardware"
                        }`}
                        key={td.propertyName + td.tab + i}
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
                            className="bubbleMenu pl-2"
                            onMouseLeave={closeBubble}
                          >
                            <div className="triangleBubbleTop"></div>
                            <div className="col-12 row mx-0 px-2 my-2">
                              <nav className="nav flex-column">
                                {item?.plannedActivity !== undefined
                                  ? Object.keys(item?.plannedActivity).map(
                                      (name, idx) => (
                                        <a
                                          onClick={() =>
                                            props.action.EditAndDetail(
                                              item.lcmEngineeringId,
                                              name
                                            )
                                          }
                                          key={name + idx}
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
                    ) : td.propertyName === "originalLcm" ? (
                      <td
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                        className={`${
                          isVisibleMajorOriginalBubble === index + 1
                            ? "majorHardware hasModal"
                            : "majorHardware"
                        }`}
                        key={td.propertyName + td.tab + i}
                      >
                        <div
                          className="majorHardware"
                          onClick={() =>
                            setIsVisibleMajorOriginalBubble(
                              toggleState(
                                index + 1,
                                isVisibleMajorOriginalBubble
                              )
                            )
                          }
                        >
                          {item?.originalLcm !== undefined
                            ? Object.keys(item?.originalLcm).map(
                                (name, index) =>
                                  index === 0 ? (
                                    <a
                                      className=""
                                      key={name}
                                      dangerouslySetInnerHTML={{
                                        __html:
                                          (item?.originalLcm &&
                                            item?.originalLcm[name]) ??
                                          "",
                                      }}
                                    ></a>
                                  ) : null
                              )
                            : "---"}
                        </div>
                        {isVisibleMajorOriginalBubble === index + 1 ? (
                          <div
                            className="bubbleMenu pl-2"
                            onMouseLeave={closeBubble}
                          >
                            <div className="triangleBubbleTop"></div>
                            <div className="col-12 row mx-0 px-2 my-2">
                              <nav className="nav flex-column">
                                {item?.originalLcm !== undefined
                                  ? Object.keys(item?.originalLcm).map(
                                      (name, idx) => (
                                        <a
                                          onClick={() =>
                                            props.action.EditAndDetail(
                                              +name,
                                              name,
                                              true,
                                              "originalLcm"
                                            )
                                          }
                                          key={name + idx}
                                          className="text-white fakeLink"
                                          dangerouslySetInnerHTML={{
                                            __html:
                                              (item?.originalLcm &&
                                                item?.originalLcm[name]) ??
                                              "",
                                          }}
                                        ></a>
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
                        td.propertyName === "lastModified"
                          ? item["lastModifiedValue"]
                          : item[td.propertyName],
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
                    )
                  )}
                <td className="actions">
                  {!readonly && !props.archivedMode && (
                    <div className="d-inline mx-2 cursor-pointer">
                      <img
                        className="dropdown_trigger"
                        src={require("../../img/options_dots.png")}
                        style={{ cursor: "pointer", padding: "10px" }}
                        onClick={(e) => {
                          toggleDropdown(
                            item.lcmEngineeringId,
                            item.plannedActivity,
                            item.orphan,
                            item.isLcmAncillaryData,
                            e,
                            item.opCo
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

export default LcmEngineeringOperational;
