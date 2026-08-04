import React, { SetStateAction, useEffect, useState, useRef } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import { ApiCallWithErrorHandling } from "../../Business/Common/CommonBusiness";
import { DesignAspectApi } from "../../Business/DesignAspectsBusiness";
import { stateConfirm, DataModalConfirm } from "../../Model/Common";

import {
  DesignAspectDtoGrid,
  DesignAspectQueryObjectGrid,
} from "../../Model/DesignAspects";
import { GetFilterColumDesignAspect } from "../../Redux/Action/DesignAspect/DesignAspectGridAction";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { QueryObjectGrid, RenderDetail } from "../../Model/Common";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import { toggleState } from "../../Hook/Common";
import { Dropdown } from "react-bootstrap";
import ModalConfirm from "../../Components/ModalConfirm";
import setLoader from "../../Redux/Action/LoaderAction";
import { useLocation, useNavigate } from "react-router";
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
  };
  data: DesignAspectDtoGrid[] | undefined;
  pagination: DesignAspectQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  dcfName?: string;
  orphanColor?: boolean;
  readonly?: boolean;
  isArchived?: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const DesignAspectsGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<DesignAspectDtoGrid[] | undefined>([]);
  const getFiltersData = (state: RootState) =>
    state.designAspectGridReducer.filter;
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
  } = useFilterTableCrud<DesignAspectQueryObjectGrid>(
    props.action.Filter,
    undefined,
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
  const toggleDropdown = (rowId, plannedActivity, orphan, event) => {
    setDropdownStates(() => ({
      rowId,
      plannedActivity,
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

  const [isVisibleMajorBubble, setIsVisibleMajorBubble] = useState(0);
  const closeBubble = () => {
    setIsVisibleMajorBubble(0);
  };

  const location: any = useLocation();

  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);

  const ResetDesignAspect = (id: number) => {
    setConfirm({
      title: "Confirm",
      message: "Are you sure you want to reset?",
      button: "Confirm",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => setConfirm(stateConfirm),
        confirm: () => confirmReset(id),
      },
    });
  };

  const confirmReset = async (id: number) => {
    setLoader("ADD", "ResetDesignAspect");

    let api = new DesignAspectApi();

    const result = await ApiCallWithErrorHandling<Promise<string>>(() =>
      api.designAspectReset(id)
    );

    setLoader("REMOVE", "ResetDesignAspect");
    setConfirm(stateConfirm);
  };

  return (
    <>
      <ModalConfirm data={confirm} />

      <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
        <div
          className="mx-0 px-0 flex-row table-container"
          style={{ position: "relative" }}
        >
          {!props?.readonly && !props.isArchived && dropdownStates["rowId"] && (
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
                  dropdownStates["rowId"]
                    ? "dropdown-menu show"
                    : "dropdown-menu"
                }`}
              >
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
                    onClick={() =>
                      props.action.EditNotDetail(dropdownStates["rowId"])
                    }
                  >
                    Edit
                  </Dropdown.Item>
                  <Dropdown.Item
                    onClick={() => ResetDesignAspect(dropdownStates["rowId"]!)}
                  >
                    Reset
                  </Dropdown.Item>
                  <Dropdown.Item
                    onClick={() =>
                      props.action.onDelete(
                        dropdownStates["rowId"],
                        dropdownStates["rowId"]
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
                      item.propertyName === "id" ? "DA Index" : undefined,
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
                  key={item.id}
                  onDoubleClick={(e) =>
                    toggleDropdown(
                      item.id,
                      item.plannedActivity,
                      item.orphan,
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
                          key={td.propertyName + td.tab + i}
                          ref={(ref) => {
                            if (i === 0) thRefs.current[0] = ref;
                            else if (i <= 2) thRefs.current[i] = ref;
                          }}
                        >
                          <div
                            className={!props.isArchived ? "majorHardware" : ""}
                            onClick={() =>
                              !props.isArchived &&
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
                                        (name, idx) => (
                                          <a
                                            onClick={() =>
                                              props.action.EditAndDetail(
                                                item.id,
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
                    {!props?.readonly && !props.isArchived && (
                      <div className="d-inline mx-2 cursor-pointer">
                        <img
                          className="dropdown_trigger"
                          src={require("../../img/options_dots.png")}
                          style={{ cursor: "pointer", padding: "10px" }}
                          onClick={(e) => {
                            toggleDropdown(
                              item.id,
                              item.plannedActivity,
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
    </>
  );
};

export default DesignAspectsGrid;
