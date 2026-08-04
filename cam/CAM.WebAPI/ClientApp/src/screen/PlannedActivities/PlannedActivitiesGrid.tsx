import React, { SetStateAction, useEffect, useState, useRef } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { QueryObjectGrid, RenderDetail } from "../../Model/Common";
import TH from "../../Components/TableCrud/TableCrudTH";
import {
  PlannedActivityDtoGrid,
  PlannedActivityQueryObjectGrid,
} from "../../Model/PlannedActivity";
import { GetFilterColumPlannedActivity } from "../../Redux/Action/PlannedActivity/PlannedActivityGridAction";
import { Link } from "react-router-dom";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import { Dropdown, Modal } from "react-bootstrap";
import ManageMigration from "./ManageMigrationModal";
import UpdatePlannedActivityStatusModal from "./UpdatePlannedActivityStatusModal";
import { useAuth } from "./../../Hook/useAuth";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { calculateBodyWidths } from "../../Utils/gridFunction";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import IconButton from "@mui/material/IconButton";
import { IoClose } from "react-icons/io5";

interface Props {
  action: {
    onDelete(id: number | undefined): any;
    Edit(id: number | undefined): any;
    Restore(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    refresh(): any;
  };
  data: PlannedActivityDtoGrid[] | undefined;
  pagination: PlannedActivityQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
  forLcm: boolean;
  forReport: boolean;
  forNetworkElement: boolean;
  forDesignAspect: boolean;
  forServicePlan: boolean;
  isArchived: boolean;
  isLandingRedirect?: any;
}

let firstIndex, secondIndex, thirdIndex;
const PlannedActivitiesGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<PlannedActivityDtoGrid[] | undefined>([]);
  const getFiltersData = (state: RootState) =>
    state.plannedActivityGridReducer.filter;
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
  } = useFilterTableCrud<PlannedActivityQueryObjectGrid>(
    props.action.Filter,
    undefined,
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

  useEffect(() => {
    setData(props.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, []);

  //UPDATE DATA
  useEffect(() => {
    setData(props?.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
    if (props.isLandingRedirect && props?.isLandingRedirect?.landingRedirect) {
      props?.isLandingRedirect?.paId &&
        openModalStatus(props?.isLandingRedirect?.paId, 0);
    }
  }, [props.data]);

  const [dropdownStates, setDropdownStates] = useState({});

  const [dropdownPosition, setDropdownPosition] = useState<{
    left: number;
    top: number;
  }>({ left: 0, top: 0 });

  const dropdownRef = useRef<HTMLDivElement>(null);

  const [dropdownVisible, setDropdownVisible] = useState(false);
  // Function to toggle dropdown state for a specific row
  const toggleDropdown = (
    rowId,
    plannedActivityId,
    designAspectId,
    servicePlanid,
    networkElementAsPlannedId,
    deliveryStatusId,
    plannedActivityTypeFor,
    event
  ) => {
    setDropdownVisible(true);
    setDropdownStates(() => ({
      rowId,
      plannedActivityId,
      designAspectId,
      servicePlanid,
      networkElementAsPlannedId,
      deliveryStatusId,
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
        setDropdownVisible(false);
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

  const [isVisibleModalManage, setIsVisibleModalManage] =
    useState<boolean>(false);
  const [plannedActivityIdToManage, setPlannedActivityIdToManage] =
    useState<number>();
  const [plannedActivityTypeFor, setplannedActivityTypeFor] =
    useState<number>();
  const [isVisibleModalStatus, setIsVisibleModalStatus] =
    useState<boolean>(false);

  const getManageMigrationData = async (id: number) => {
    setPlannedActivityIdToManage(id);
    setIsVisibleModalManage(true);
  };

  const openModalStatus = (id: number, plannedActivityTypeFor: number) => {
    setPlannedActivityIdToManage(id);
    setplannedActivityTypeFor(plannedActivityTypeFor);
    setIsVisibleModalStatus(true);
  };

  return (
    <div className="listaApparatiContainer row mx-0 col-12 p-0 d-flex justify-content-center">
      <Modal
        show={isVisibleModalManage}
        backdrop="static"
        keyboard={false}
        size="xl"
        onHide={() => setIsVisibleModalManage(false)}
      >
        <Modal.Header className="d-flex justify-content-center" closeButton>
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0">Manage Migration</h4>
            </div>
          </div>
        </Modal.Header>
        <Modal.Body>
          <ManageMigration
            isFromPlannedActivityModal={false}
            action={{ setIsVisibleModalManage }}
            plannedActivityId={plannedActivityIdToManage}
          ></ManageMigration>
        </Modal.Body>
      </Modal>

      <Dialog
        open={isVisibleModalStatus}
        onClose={() => setIsVisibleModalStatus(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        maxWidth="lg"
        scroll="body"
        fullWidth={true}
        slotProps={{ paper: { sx: { borderRadius: "15px" } } }}
      >
        <DialogTitle className="d-flex justify-content-center">
          <div className="col-12 px-0">
            <div className="col-12">
              <h4 className="mb-0">Update Planned Activity Status</h4>
            </div>
          </div>
        </DialogTitle>
        <IconButton
          aria-label="close"
          onClick={() => setIsVisibleModalStatus(false)}
          sx={{
            position: "absolute",
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <IoClose size={25} />
        </IconButton>
        <DialogContent>
          <UpdatePlannedActivityStatusModal
            isFromPlannedActivityModal={false}
            isFromLandingPage={
              props?.isLandingRedirect?.landingRedirect ?? false
            }
            action={{
              setIsVisibleModalStatus: setIsVisibleModalStatus,
              Refresh: () => props.action.refresh(),
            }}
            planningActivityDetailsResourceId={plannedActivityIdToManage}
            plannedActivityTypeForEnum={plannedActivityTypeFor}
          />
        </DialogContent>
      </Dialog>

      <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
        <div
          className="mx-0 px-0 flex-row table-container"
          style={{ position: "relative" }}
        >
          {!readonly &&
            !props.isArchived &&
            !props.forReport &&
            dropdownVisible && (
              <Dropdown
                className="d-inline mx-2"
                show={dropdownVisible}
                ref={dropdownRef}
                style={
                  dropdownVisible
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
                    dropdownVisible ? "dropdown-menu show" : "dropdown-menu"
                  }`}
                >
                  <>
                    <Dropdown.Item>
                      {dropdownStates["rowId"] !== undefined &&
                      dropdownStates["rowId"] !== 0 &&
                      dropdownStates["rowId"] !== null ? (
                        <div className="d-flex flex-row">
                          <Link
                            style={{
                              color: "#333333",
                              width: "100%",
                            }}
                            to={{
                              pathname: "/lcmEngineering",
                              search: "id=" + dropdownStates["rowId"],
                            }}
                            state={{
                              id: dropdownStates["rowId"],
                              tab: "plannedActivities",
                              prevPage: "plannedActivities",
                              idDetail: dropdownStates["plannedActivityId"],
                            }}
                          >
                            Edit
                          </Link>
                        </div>
                      ) : dropdownStates["designAspectId"] !== undefined &&
                        dropdownStates["designAspectId"] !== 0 &&
                        dropdownStates["designAspectId"] !== null ? (
                        <div className="d-flex flex-row">
                          <Link
                            style={{
                              color: "#333333",
                              width: "100%",
                            }}
                            to={{
                              pathname: "/designAspect",
                              search: "id=" + dropdownStates["designAspectId"],
                            }}
                            state={{
                              id: dropdownStates["designAspectId"],
                              tab: "plannedActivities",
                              prevPage: "plannedActivities",
                              idDetail: dropdownStates["plannedActivityId"],
                            }}
                          >
                            Edit
                          </Link>
                        </div>
                      ) : dropdownStates["servicePlanid"] !== undefined &&
                        dropdownStates["servicePlanid"] !== 0 &&
                        dropdownStates["servicePlanid"] !== null ? (
                        <div className="d-flex flex-row">
                          <Link
                            style={{
                              color: "#333333",
                              width: "100%",
                            }}
                            to={{
                              pathname: "/servicelevel",
                              search: "id=" + dropdownStates["servicePlanid"],
                            }}
                            state={{
                              id: dropdownStates["servicePlanid"],
                              tab: "plannedActivities",
                              prevPage: "plannedActivities",
                              idDetail: dropdownStates["plannedActivityId"],
                            }}
                          >
                            Edit
                          </Link>
                        </div>
                      ) : (
                        <div className="d-flex flex-row">
                          <Link
                            style={{ color: "#333333", width: "100%" }}
                            to={{
                              pathname: "/asplanned",
                              search:
                                "id=" +
                                dropdownStates["networkElementAsPlannedId"],
                            }}
                            state={{
                              id: dropdownStates["networkElementAsPlannedId"],
                              tab: "plannedActivities",
                              prevPage: "plannedActivities",
                              idDetail: dropdownStates["plannedActivityId"],
                            }}
                          >
                            Edit
                          </Link>
                        </div>
                      )}
                    </Dropdown.Item>
                    {dropdownStates["plannedActivityId"] != undefined &&
                      dropdownStates["rowId"] != undefined &&
                      dropdownStates["rowId"] != 0 && (
                        <Dropdown.Item>
                          <button
                            disabled={
                              !(
                                dropdownStates["deliveryStatusId"] ===
                                  "RFS Achieved" ||
                                dropdownStates["deliveryStatusId"] ===
                                  "FSI Achieved" ||
                                dropdownStates["deliveryStatusId"] ===
                                  "Rollout Complete"
                              )
                            }
                            type="button"
                            title="Manage Migration"
                            className="btn btn-link p-0"
                            style={{ color: "#333333" }}
                            onClick={() =>
                              dropdownStates["plannedActivityId"] &&
                              getManageMigrationData(
                                dropdownStates["plannedActivityId"]
                              )
                            }
                          >
                            Manage Migrations
                          </button>
                        </Dropdown.Item>
                      )}
                    {dropdownStates["plannedActivityId"] !== undefined &&
                      dropdownStates["rowId"] !== undefined &&
                      dropdownStates["rowId"] !== null &&
                      dropdownStates["rowId"] !== 0 &&
                      !dropdownStates["designAspectId"] && (
                        <Dropdown.Item>
                          <button
                            type="button"
                            title="Update Planned Activity Status"
                            className="btn btn-link p-0"
                            style={{ color: "#333333" }}
                            onClick={() =>
                              dropdownStates["plannedActivityId"] &&
                              openModalStatus(
                                dropdownStates["plannedActivityId"],
                                dropdownStates["plannedActivityTypeFor"]!
                              )
                            }
                          >
                            Update Planned Activity Status
                          </button>
                        </Dropdown.Item>
                      )}

                    <Dropdown.Item
                      onClick={() =>
                        props.action.onDelete(
                          dropdownStates["plannedActivityId"]
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
                  .sort((a, b) => a.order - b.order)
                  .filter((x) => x.show)
                  .map((item, i) =>
                    item.propertyName === "activityDetailss" ? (
                      <TH
                        spanClassName={item.colorHeader ?? ""}
                        propertyName={item.propertyName}
                        action={thActionDate}
                        key={i}
                        isVisibleFiltriString={isVisibleFiltriString}
                        setupDuplicates={false}
                        hideFilter={false}
                        freezeHeader={true}
                      />
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
                        false,
                        undefined,
                        undefined,
                        undefined,
                        item.propertyName === "budgetAvailability"
                          ? "INDB?"
                          : item.propertyName === "localApproval"
                          ? "INSAP?"
                          : item.propertyName === "designComponentId"
                          ? "Planned Design Component"
                          : undefined,
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
                  key={index}
                  onDoubleClick={(e) =>
                    toggleDropdown(
                      item.lcmEngineeringId,
                      item.plannedActivityId,
                      item.designAspectId,
                      item.servicePlanid,

                      item.networkElementAsPlannedId,
                      item.deliveryStatusId,
                      item.plannedActivityTypeFor,
                      e
                    )
                  }
                >
                  {props.renderGrid
                    .sort((a, b) => a.order - b.order)
                    .filter((x) => x.show)
                    .map((td, i) =>
                      td.propertyName === "activityDetails" ? (
                        <td
                          className=" "
                          dangerouslySetInnerHTML={{
                            __html: item[td.propertyName] ?? "---",
                          }}
                          key={i}
                          ref={(ref) => {
                            if (i === 0) thRefs.current[0] = ref;
                            else if (i <= 2) thRefs.current[i] = ref;
                          }}
                        ></td>
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
                    {!readonly && !props.forReport && !props.isArchived && (
                      <div className="d-inline mx-2 cursor-pointer">
                        <img
                          className="dropdown_trigger"
                          src={require("../../img/options_dots.png")}
                          style={{ cursor: "pointer", padding: "10px" }}
                          onClick={(e) => {
                            toggleDropdown(
                              item.lcmEngineeringId,
                              item.plannedActivityId,
                              item.designAspectId,
                              item.servicePlanid,

                              item.networkElementAsPlannedId,
                              item.deliveryStatusId,
                              item.plannedActivityTypeFor,
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
    </div>
  );
};

export default PlannedActivitiesGrid;
