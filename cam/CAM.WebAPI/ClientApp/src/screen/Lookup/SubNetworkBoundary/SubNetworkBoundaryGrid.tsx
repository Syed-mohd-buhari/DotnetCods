import React, { useState, useEffect, SetStateAction, useRef } from "react";
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

import {
  SubNetworkBoundaryGridDto,
  SubNetworkBoundaryQueryDto,
} from "../../../Model/LookUp/SubnetworkBoundry";
import { GetFilterColumSubNetworkBoundary } from "../../../Redux/Action/LookUp/SubNetworkBoundary/SubNetworkBoundryGridAction";
import { Dropdown } from "react-bootstrap";
import { GetProductNameByVodafoneName } from "../../../Redux/Action/LookUp/VodafoneName/VodafoneNameCommonAction";
import { toggleState } from "../../../Hook/Common";
import ThreeDot from "../../../Components/TableCrud/ThreeDot";
import { calculateBodyWidths } from "../../../Utils/gridFunction";

interface Props {
  action: {
    onDelete(id: number | undefined): any;
    Edit(id: number | undefined): any;
    Filter(obj: SetStateAction<SubNetworkBoundaryQueryDto> | undefined): any;
  };
  data: SubNetworkBoundaryGridDto[] | undefined;
  pagination: SubNetworkBoundaryQueryDto | undefined;
  orphanColor?: boolean;
  renderGrid: RenderDetail[];
  readonly?: boolean;
  isPopup?: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const SubNetworkBoundaryGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<SubNetworkBoundaryGridDto[] | undefined>([]);
  const [isVisibleVodafoneBubble, setIsVisibleVodafoneBubble] = useState(0);
  const [relatedVodafoneNames, setRelatedVodafoneNames] = useState([]);
  const getFiltersData = (state: RootState) =>
    state.subNetworkBoundaryGridReducer.filter;
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
  } = useFilterTableCrud<SubNetworkBoundaryQueryDto>(
    props.action.Filter,
    GetFilterColumSubNetworkBoundary,
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

  //CARICAMENTO INIZIALE
  useEffect(() => {
    setData(props.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, []);

  ///UPDATE DATA
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
  const containerRef = useRef<HTMLTableElement>(null);

  // Function to toggle dropdown state for a specific row
  const toggleDropdown = (rowId, isDefault, orphan, event) => {
    setDropdownStates(() => ({
      rowId,
      isDefault,
      orphan,
    }));
    if (!props.isPopup) {
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
    } else {
      const popupElement = containerRef.current;
      if (popupElement) {
        // popup position
        const popupRect = popupElement.getBoundingClientRect();
        const popupWidth = Math.floor(popupRect.width);
        const popupHeight = popupRect.height;
        const popupLeft = popupRect.left + window.scrollX;
        const popupTop = popupRect.top + window.scrollY;

        const dropdownWidth = 200;
        const dropdownHeight = 20;

        // click position
        const clickLeft = event.clientX + window.scrollX;
        const clickTop = event.clientY + window.scrollY;

        const relativeClickLeft = clickLeft - popupLeft;
        const relativeClickTop = clickTop - popupTop;

        const maxLeft = popupWidth - dropdownWidth;
        const maxTop = popupHeight - dropdownHeight;

        const dropdownLeft = Math.min(Math.max(relativeClickLeft, 0), maxLeft);
        const dropdownTop = Math.min(Math.max(relativeClickTop, 0), maxTop);

        setDropdownPosition({
          left: dropdownLeft,
          top: dropdownTop,
        });
      }
    }
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
  const getRelatedVodafoneNames = async (vodafoneId, index) => {
    const result = await GetProductNameByVodafoneName(vodafoneId);
    setRelatedVodafoneNames(result);
    setIsVisibleVodafoneBubble(toggleState(index + 1, isVisibleVodafoneBubble));
  };
  const closeVodafoneBubble = () => {
    setIsVisibleVodafoneBubble(0);
  };
  return (
    <div className="listaApparatiContainer row mx-0 col-12 p-0 d-flex justify-content-center ">
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
                    top: props.isPopup
                      ? `${dropdownPosition.top}px`
                      : `${dropdownPosition.top - 150}px`,
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
                  disabled={dropdownStates["isDefault"] === true}
                  onClick={() => props.action.Edit(dropdownStates["rowId"])}
                >
                  Edit
                </Dropdown.Item>
                <Dropdown.Item
                  disabled={dropdownStates["isDefault"] === true}
                  onClick={() => props.action.onDelete(dropdownStates["rowId"])}
                >
                  Delete
                </Dropdown.Item>
              </>
            </div>
          </Dropdown>
        )}
        <table
          className="w-100 table-responsive table-thead-sticky"
          tabIndex={-1}
          ref={containerRef}
        >
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
                    props.isPopup ? false : true
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
                  key={item.subNetworkBoundaryId}
                  className={`dati ${
                    props?.orphanColor && item.orphan ? "orphan" : null
                  }`}
                  onDoubleClick={(e) =>
                    toggleDropdown(
                      item.subNetworkBoundaryId,
                      item.default,
                      item.orphan,
                      e
                    )
                  }
                >
                  {props.renderGrid
                    .sort((a, b) => a.order - b.order)
                    .filter((x) => x.show)
                    .map((td, i) =>
                      td.propertyName === "vodafoneName" ? (
                        <td
                          className={`${
                            isVisibleVodafoneBubble == index + 1
                              ? "majorHardware hasModal"
                              : "majorHardware"
                          }`}
                          key={`${td.propertyName}${i}`}
                          ref={(ref) => {
                            if (props.isPopup) return;
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
                          props.isPopup ? undefined : i,
                          props.isPopup ? undefined : thRefs,
                          props.isPopup ? undefined : thRefss
                        )
                      )
                    )}
                  {/* <div className="d-flex flex-row justify-content-end">
                      <button
                        type="button"
                        className="btn btn-link "
                        onClick={() =>
                          props.action.Edit(item.subNetworkBoundaryId)
                        }
                      >
                        <img
                          className="btnEdit"
                          src={require("../../../img/edit.png")}
                        />
                      </button>
                      <button
                        type="button"
                        className="btn btn-link "
                        onClick={() =>
                          props.action.onDelete(item.subNetworkBoundaryId)
                        }
                      >
                        <img
                          className="btnEdit"
                          src={require("../../../img/delete.png")}
                        />
                      </button>
                    </div> */}
                  <td className="actions">
                    {!props.readonly && (
                      <div className="d-inline mx-2 cursor-pointer">
                        <img
                          className="dropdown_trigger"
                          src={require("../../../img/options_dots.png")}
                          style={{ cursor: "pointer", padding: "10px" }}
                          onClick={(e) => {
                            toggleDropdown(
                              item.subNetworkBoundaryId,
                              item.default,
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

export default SubNetworkBoundaryGrid;
