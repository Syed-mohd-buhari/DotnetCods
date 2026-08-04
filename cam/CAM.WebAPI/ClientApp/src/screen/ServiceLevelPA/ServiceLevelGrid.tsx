import React, { SetStateAction, useEffect, useState, useRef } from "react";
import { Dropdown, Modal } from "react-bootstrap";
import { useSelector } from "react-redux";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import {
  DataModalConfirm,
  QueryObjectGrid,
  RenderDetail,
  stateConfirm,
} from "../../Model/Common";
import { GetFilterColumOrganizationInfo } from "../../Redux/Action/OrganizationInfo/OrganizationInfoGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import { useAuth } from "../../Hook/useAuth";
import ModalConfirm from "../../Components/ModalConfirm";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { calculateBodyWidths } from "../../Utils/gridFunction";
import {
  OrganizationInfoDtoGrid,
  OrganizationInfoQueryObjectGrid,
} from "../../Model/OrganizationInfo";
import {
  ServicePlanDtoGrid,
  ServicePlanQueryObjectGrid,
} from "../../Model/ServicePlan";
import { toggleState } from "../../Hook/Common";
import { GetFilterColumServicePlan } from "../../Redux/Action/ServicePlan/ServicePlanGridAction";

interface Props {
  action: {
    onDelete(id: number | undefined, orphan?: boolean): any;
    onEdit(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
  };
  data: ServicePlanDtoGrid[] | undefined;
  pagination: ServicePlanQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const ServiceLevelGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<ServicePlanDtoGrid[] | undefined>([]);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [isVisibleMajorOriginalBubble, setIsVisibleMajorOriginalBubble] =
    useState(0);

  const { readonly, isPermesso } = useAuth();
  const getFiltersData = (state: RootState) =>
    state.servicePlanGridReducer.filter;
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
  } = useFilterTableCrud<ServicePlanQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumServicePlan,
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

  //UPDATE DATA
  useEffect(() => {
    setData(props?.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, [props.data]);

  const [dropdownStates, setDropdownStates] = useState({});

  const [dropdownPosition, setDropdownPosition] = useState<{
    left: number;
    top: number;
  }>({ left: 0, top: 0 });

  const dropdownRef = useRef<HTMLDivElement>(null);

  // Function to toggle dropdown state for a specific row
  const toggleDropdown = (rowId, item, event) => {
    setDropdownStates(() => ({
      rowId,
      item,
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

  const Cancel = (msg) => {
    setConfirm({
      title: "Warning!",
      message: msg,
      cancelText: "Ok",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => {
          setConfirm(stateConfirm);
        },
      },
    });
  };

  return (
    <>
      <ModalConfirm data={confirm} showHyperLink={false} />
      <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
        <div className="mx-0 px-0 flex-row" style={{ position: "relative" }}>
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
                  dropdownStates["rowId"]
                    ? "dropdown-menu show"
                    : "dropdown-menu"
                }`}
              >
                <>
                  <Dropdown.Item
                    onClick={() => props.action.onEdit(dropdownStates["rowId"])}
                  >
                    Edit
                  </Dropdown.Item>
                  <Dropdown.Item
                    onClick={() =>
                      props.action.onDelete &&
                      props.action.onDelete(dropdownStates["rowId"])
                    }
                  >
                    Delete
                  </Dropdown.Item>
                </>
              </div>
            </Dropdown>
          )}
          <table className="table-responsive" tabIndex={-1}>
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
                  className={`dati `}
                  key={index + "-" + item.servicePlanId}
                  onDoubleClick={(e) =>
                    toggleDropdown(item.servicePlanId, item, e)
                  }
                >
                  {props.renderGrid
                    .filter((x) => x.show)
                    .sort((a, b) => a.order - b.order)
                    .map((td, i) =>
                      td.propertyName === "plannedActivity" ? (
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
                            // onClick={() =>
                            //   setIsVisibleMajorBubble(
                            //     toggleState(index + 1, isVisibleMajorBubble)
                            //   )
                            // }
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
                              // onMouseLeave={closeBubble}
                            >
                              <div className="triangleBubbleTop"></div>
                              <div className="col-12 row mx-0 px-2 my-2">
                                <nav className="nav flex-column">
                                  {item?.plannedActivity !== undefined
                                    ? Object.keys(item?.plannedActivity).map(
                                        (name, idx) => (
                                          <span
                                            // onClick={() =>
                                            //   props.action.EditAndDetail(
                                            //     item.lcmEngineeringId,
                                            //     name
                                            //   )
                                            // }
                                            key={name + idx}
                                            className="text-white fakeLink"
                                          >
                                            {item?.plannedActivity &&
                                              item?.plannedActivity[name]}
                                          </span>
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
                            toggleDropdown(item.servicePlanId, item, e);
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

export default ServiceLevelGrid;
