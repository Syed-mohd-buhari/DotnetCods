import React, { SetStateAction, useEffect, useState, useRef } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  MajorSoftwareBuildDtoGrid,
  MajorSoftwareBuildQueryObjectGrid,
} from "../../Model/MajorSoftwareBuild";
import { GetFilterColumMajorSoftwareBuild } from "../../Redux/Action/MajorSoftwareBuild/MajorSoftwareBuildGridAction";
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

interface Props {
  action: {
    onDelete(id: number | undefined, orphan?: boolean): any;
    Edit(id: number | undefined): any;
    Restore(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    setIsFiltriAttivati(value: boolean): any;
    GetMajorSoftwareCloneData(value: number): any;
  };
  data: MajorSoftwareBuildDtoGrid[] | undefined;
  pagination: MajorSoftwareBuildQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
  readonly?: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const MajorSoftwareGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<MajorSoftwareBuildDtoGrid[] | undefined>([]);
  const tourStarted = useSelector(
    (state: RootState) => state.tourGuide.startGuideTour
  );
  const getFiltersData = (state: RootState) =>
    state.majorSoftwareBuildGridReducer.filter;
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
  } = useFilterTableCrud<MajorSoftwareBuildQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumMajorSoftwareBuild,
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
                      props.action.GetMajorSoftwareCloneData(
                        dropdownStates["rowId"] ?? 0
                      )
                    }
                    id="tour_swVersionButton"
                  >
                    Upgrade Software Version
                  </Dropdown.Item>
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
                .sort((a, b) => a.order - b.order)
                .filter((x) => x.show)
                .map((item, i) =>
                  item.propertyName === "thirdPartySoftwareComponents" ? (
                    <TH
                      propertyName="thirdPartySoftwareComponents"
                      action={thAction}
                      key="thirdPartySoftwareComponents"
                      isVisibleFiltriString={isVisibleFiltriString}
                      freezeHeader={true}
                    ></TH>
                  ) : (
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
                key={item.majorSoftwareBuildId}
                onDoubleClick={(e) =>
                  toggleDropdown(
                    item.majorSoftwareBuildId,
                    item.deleted,
                    item.orphan,
                    e
                  )
                }
                id={index === 0 ? "tourGrid_doubleClickRow" : "tourGrid"}
              >
                {props.renderGrid
                  .sort((a, b) => a.order - b.order)
                  .filter((x) => x.show)
                  .map((td, i) =>
                    td.propertyName === "thirdPartySoftwareComponents" ? (
                      <td className=" " key="thirdPartyGrid"></td>
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
                            item.majorSoftwareBuildId,
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
export default MajorSoftwareGrid;
