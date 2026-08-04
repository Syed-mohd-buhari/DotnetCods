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
import {
  UserManagementDtoGrid,
  UserManagementQueryObjectGrid,
} from "../../Model/UserManagement";
import { GetFilterColumUserManagement } from "../../Redux/Action/UserManagement/UserManagementGridAction";
import { RootState } from "../../Redux/Store/rootStore";
import { useAuth } from "../../Hook/useAuth";
import ModalConfirm from "../../Components/ModalConfirm";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { calculateBodyWidths } from "../../Utils/gridFunction";

interface Props {
  action: {
    DeleteUser(id: number | undefined, orphan?: boolean): any;
    EditUser(details: any): any;
    ViewDetails(details: any): any;
    UserActivation(id: number | undefined, activeStatus: any): any;
    Restore(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
    setIsFiltriAttivati(value: boolean): any;
    closeModal(): any;
  };
  data: UserManagementDtoGrid[] | undefined;
  pagination: UserManagementQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const UserManagementGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<UserManagementDtoGrid[] | undefined>([]);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const { readonly, isPermesso } = useAuth();
  const getFiltersData = (state: RootState) =>
    state.userManagementGridReducer.filter;
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
  } = useFilterTableCrud<UserManagementQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumUserManagement,
    props.pagination
  );

  const [selectAll, setSelectAll] = useState<boolean>(false);
  const [selectedRows, setSelectedRows] = useState<any>([]);
  const [isVisibleModalStatus, setIsVisibleModalStatus] =
    useState<boolean>(false);

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
  const toggleDropdown = (rowId, item, active, aspNetUserRoleId, event) => {
    setDropdownStates(() => ({
      rowId,
      item,
      active,
      aspNetUserRoleId,
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
                    onClick={() =>
                      props.action.ViewDetails(dropdownStates["item"])
                    }
                  >
                    View Details
                  </Dropdown.Item>
                  {/* <Dropdown.Item
                            onClick={() =>
                              props.action.EditUser(dropdownStates["item"])
                            }
                          >
                            Edit
                          </Dropdown.Item> */}
                  {/* <Dropdown.Item
                            onClick={() =>
                              props.action.DeleteUser(dropdownStates["aspNetUserRoleId"])
                            }
                          >
                            Delete
                          </Dropdown.Item> */}
                  <Dropdown.Item
                    onClick={() =>
                      props.action.UserActivation(
                        dropdownStates["rowId"],
                        dropdownStates["active"]
                      )
                    }
                  >
                    {dropdownStates["active"] ? "De-Activate" : "Activate"}
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
                  key={index + "-" + item.userId}
                  onDoubleClick={(e) =>
                    toggleDropdown(
                      item.userId,
                      item,
                      item.active,
                      item.aspNetUserRoleId,
                      e
                    )
                  }
                >
                  {props.renderGrid
                    .filter((x) => x.show)
                    .sort((a, b) => a.order - b.order)
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
                              item.userId,
                              item,
                              item.active,
                              item.aspNetUserRoleId,
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

export default UserManagementGrid;
