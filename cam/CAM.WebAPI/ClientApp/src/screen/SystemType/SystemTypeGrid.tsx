import React, { useState, useEffect, SetStateAction, useRef } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { toggleState } from "../../Hook/Common";
import {
  SystemTypeDtoGrouped,
  SystemTypeQueryObjectGrid,
} from "../../Model/SystemTypeModel";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import { GetFilterColumSystemType } from "../../Redux/Action/SystemType/SystemTypeGridAction";
import { Link } from "react-router-dom";
import { QueryObjectGrid, RenderDetail } from "../../Model/Common";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import { Dropdown } from "react-bootstrap";
import { GetProductNameByVodafoneName } from "../../Redux/Action/LookUp/VodafoneName/VodafoneNameCommonAction";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { calculateBodyWidths } from "../../Utils/gridFunction";

interface Props {
  action: {
    onDelete(id: number | undefined, orphan?: boolean): any;

    Edit(id: number | undefined): any | void;
    Restore(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any | void;
    setIsFiltriAttivati(value: boolean): any;
  };
  data: SystemTypeDtoGrouped[] | undefined;
  pagination: SystemTypeQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
  readonly?: boolean;
}
let firstIndex, secondIndex, thirdIndex;
const Structure: React.FC<Props> = (props) => {
  const [data, setData] = useState<SystemTypeDtoGrouped[] | undefined>([]);

  const getFiltersData = (state: RootState) =>
    state.systemTypeGridReducer.filter;
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
  } = useFilterTableCrud<SystemTypeQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumSystemType,
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

  const [dropdownStates, setDropdownStates] = useState({});

  const [dropdownPosition, setDropdownPosition] = useState<{
    left: number;
    top: number;
  }>({ left: 0, top: 0 });

  const dropdownRef = useRef<HTMLDivElement>(null);

  // Function to toggle dropdown state for a specific row
  const toggleDropdown = (rowId, isDeleted, orphan, event) => {
    setDropdownStates(() => ({
      rowId,
      isDeleted,
      orphan,
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

  //UPDATE DATA
  useEffect(() => {
    setData(props?.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, [props.data]);

  useEffect(() => {
    props.action.setIsFiltriAttivati(isFiltriAttivati);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, [isFiltriAttivati]);

  const [isVisibleMajorBubble, setIsVisibleMajorBubble] = useState(0);
  const [isVisibleMajorSoftWareBubble, setIsVisibleMajorSoftWareBubble] =
    useState(0);

  const [isVisibleVodafoneBubble, setIsVisibleVodafoneBubble] = useState(0);
  const [relatedVodafoneNames, setRelatedVodafoneNames] = useState([]);
  const closeBubble = () => {
    setIsVisibleMajorBubble(0);
  };
  const closeVodafoneBubble = () => {
    setIsVisibleVodafoneBubble(0);
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
  const getRelatedVodafoneNames = async (vodafoneId, index) => {
    const result = await GetProductNameByVodafoneName(vodafoneId);
    setRelatedVodafoneNames(result);
    setIsVisibleVodafoneBubble(toggleState(index + 1, isVisibleVodafoneBubble));
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
                    onClick={() => props.action.Edit(dropdownStates["rowId"])}
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
                .filter((x) => x.show && x.tab === "Export")
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
            {data &&
              data?.map((item, index) => (
                <tr
                  className={`dati ${
                    props.orphanColor && item.orphan ? "orphan" : null
                  }`}
                  key={item.systemTypeId}
                  onDoubleClick={(e) =>
                    toggleDropdown(
                      item.systemTypeId,
                      item.deleted,
                      item.orphan,
                      e
                    )
                  }
                  id={index === 0 ? "tourGrid_doubleClickRow" : "tourGrid"}
                >
                  {props.renderGrid
                    .filter((x) => x.show && x.tab === "Export")
                    .sort((a, b) => a.order - b.order)
                    .map((td, i) =>
                      td.propertyName === "majorHardwareBuild" ? (
                        <td
                          className={`${
                            isVisibleMajorBubble == index + 1
                              ? "majorHardware hasModal"
                              : "majorHardware"
                          }`}
                          key={`${td.propertyName}${item.systemTypeId}`}
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
                            {item?.majorHardwareBuild != undefined
                              ? Object.keys(item?.majorHardwareBuild).map(
                                  (name, index) =>
                                    name ===
                                    item?.majorHardwareBuildId?.toString() ? (
                                      <a
                                        className=""
                                        key={name}
                                        dangerouslySetInnerHTML={{
                                          __html:
                                            item.majorHardwareBuildWithoutOem ??
                                            "---",
                                        }}
                                      ></a>
                                    ) : null
                                )
                              : "---"}
                          </div>
                          {isVisibleMajorBubble == index + 1 ? (
                            <div
                              className="bubbleMenu pl-2"
                              onMouseLeave={closeBubble}
                            >
                              <div className="triangleBubbleTop"></div>
                              <div className="col-12 row mx-0 px-2 my-2">
                                <nav className="nav flex-column">
                                  {item?.majorHardwareBuild != undefined
                                    ? Object.keys(item?.majorHardwareBuild).map(
                                        (name) => (
                                          <Link
                                            to={{
                                              pathname: "/majorhardware",
                                              search: "id=" + name,
                                            }}
                                            state={{
                                              id: name,
                                              tab: "MajorHardwareBuild",
                                              prevPage: "systemtype",
                                            }}
                                            className="text-white"
                                            key={name}
                                          >
                                            <label
                                              style={{ cursor: "pointer" }}
                                              className="mb-0 linkTo text-white"
                                              dangerouslySetInnerHTML={{
                                                __html:
                                                  item?.majorHardwareBuild &&
                                                  item?.majorHardwareBuild[
                                                    name
                                                  ] != undefined
                                                    ? item?.majorHardwareBuild &&
                                                      item?.majorHardwareBuild[
                                                        name
                                                      ]
                                                    : "---",
                                              }}
                                            ></label>
                                            {}
                                          </Link>
                                        )
                                      )
                                    : null}
                                </nav>
                              </div>
                            </div>
                          ) : null}
                        </td>
                      ) : td.propertyName === "majorSoftwareBuild" ? (
                        <td
                          className={`${
                            isVisibleMajorSoftWareBubble == index + 1
                              ? "majorHardware hasModal"
                              : "majorHardware"
                          }`}
                          key={`${td.propertyName}${item.systemTypeId}`}
                          ref={(ref) => {
                            if (i === 0) thRefs.current[0] = ref;
                            else if (i <= 2) thRefs.current[i] = ref;
                          }}
                        >
                          <div
                            className="majorHardware"
                            onClick={() =>
                              setIsVisibleMajorSoftWareBubble(
                                toggleState(
                                  index + 1,
                                  isVisibleMajorSoftWareBubble
                                )
                              )
                            }
                          >
                            {item?.majorSoftwareBuild != undefined ? (
                              <a
                                className=""
                                dangerouslySetInnerHTML={{
                                  __html: item.majorSoftwareBuild ?? "---",
                                }}
                              ></a>
                            ) : (
                              "---"
                            )}
                          </div>
                          {isVisibleMajorSoftWareBubble == index + 1 ? (
                            <div
                              className="bubbleMenu pl-2"
                              onMouseLeave={closeBubble}
                            >
                              <div className="triangleBubbleTop"></div>
                              <div className="col-12 row mx-0 px-2 my-2">
                                <nav className="nav flex-column">
                                  {item?.majorSoftwareBuildId != undefined ? (
                                    <Link
                                      to={{
                                        pathname: "/majorsoftware",
                                        search:
                                          "id=" + item?.majorSoftwareBuildId,
                                      }}
                                      state={{
                                        id: item?.majorSoftwareBuildId,
                                        tab: "MajorSoftwareBuild",
                                        prevPage: "systemtype",
                                      }}
                                    >
                                      <label
                                        style={{ cursor: "pointer" }}
                                        className="mb-0 linkTo text-white"
                                        dangerouslySetInnerHTML={{
                                          __html:
                                            item.majorSoftwareBuild ?? "---",
                                        }}
                                      ></label>
                                      {}
                                    </Link>
                                  ) : null}
                                </nav>
                              </div>
                            </div>
                          ) : null}
                        </td>
                      ) : td.propertyName === "vodafoneName" ? (
                        <td
                          className={`${
                            isVisibleVodafoneBubble == index + 1
                              ? "majorHardware hasModal"
                              : "majorHardware"
                          }`}
                          key={`${td.propertyName}${item.systemTypeId}`}
                          ref={(ref) => {
                            if (i === 0) thRefs.current[0] = ref;
                            else if (i <= 2) thRefs.current[i] = ref;
                          }}
                        >
                          <div
                            className="majorHardware"
                            onClick={() =>
                              getRelatedVodafoneNames(
                                item["vodafoneNameId"],
                                index
                              )
                            }
                          >
                            {item?.vodafoneName != undefined ? (
                              <a
                                className=""
                                dangerouslySetInnerHTML={{
                                  __html: item.vodafoneName ?? "---",
                                }}
                              ></a>
                            ) : (
                              "---"
                            )}
                          </div>
                          {isVisibleVodafoneBubble == index + 1 ? (
                            <div
                              className="bubbleMenu pl-2"
                              onMouseLeave={closeVodafoneBubble}
                            >
                              <div className="triangleBubbleTop"></div>
                              <div className="col-12 row mx-0 px-2 my-2">
                                <nav className="nav flex-column">
                                  {item?.vodafoneName != undefined ? (
                                    <label
                                      style={{ cursor: "pointer" }}
                                      className="mb-0  text-white"
                                    >
                                      <ol>
                                        {relatedVodafoneNames &&
                                          relatedVodafoneNames?.map(
                                            (vdnames) => <li>{vdnames}</li>
                                          )}
                                      </ol>
                                    </label>
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
                          undefined,
                          undefined,
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
                              item.systemTypeId,
                              item.deleted,
                              item.orphan,
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

export default Structure;
