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
import { RootState } from "../../Redux/Store/rootStore";
import { useAuth } from "../../Hook/useAuth";
import ModalConfirm from "../../Components/ModalConfirm";
import ThreeDot from "../../Components/TableCrud/ThreeDot";
import { calculateBodyWidths } from "../../Utils/gridFunction";
import {
  OrganizationInfoDtoGrid,
  OrganizationInfoQueryObjectGrid,
} from "../../Model/OrganizationInfo";
import { VBOMInfoDtoGrid } from "../../Model/VBOMInfo";
import { GetFilterColumnVBOMInfo } from "../../Redux/Action/VBOMInfo/VBOMInfoGridAction";
import { FaChevronDown, FaChevronRight } from "react-icons/fa";
import { calculateWidths } from "../../Components/TableCrud/TableCrudTH";
import VBOMInstanceGrid from "./VBOMInstanceGrid";
import { Box, Button, Grid, Tooltip } from "@mui/material";

interface Props {
  action: {
    Delete(type: string, id: number | undefined, alowDelete: boolean): any;
    Edit(id: number | undefined): any;
    onInfoIdChange(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
  };
  data: VBOMInfoDtoGrid[] | undefined;
  pagination: OrganizationInfoQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  infoId?: number | null;
  orphanColor?: boolean;
}

let firstIndex, secondIndex, thirdIndex;
const VBOMInfoGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<VBOMInfoDtoGrid[] | undefined>([]);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const { readonly, isPermesso } = useAuth();
  const getFiltersData = (state: RootState) => state.VBOMInfoGridReducer.filter;
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
  } = useFilterTableCrud<OrganizationInfoQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumnVBOMInfo,
    props.pagination
  );

  const [selectAll, setSelectAll] = useState<boolean>(false);
  const [selectedRows, setSelectedRows] = useState<any>([]);
  const [isVisibleModalStatus, setIsVisibleModalStatus] =
    useState<boolean>(false);
  const [expandedRows, setExpandedRows] = useState<any[]>([]);

  const handleToggleExpand = (id: any) => {
    setExpandedRows((prev) =>
      prev.includes(id) ? prev.filter((rowId) => rowId !== id) : [id]
    );
  };
  const THRefs = useRef<HTMLTableCellElement | null>(null);
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
    } else if (index !== 0 && index <= 2) {
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
  useEffect(() => {
    calculateWidths(THRefs);
  }, [props]);

  const infoRenderList = [
    "vnfNameId",
    "vnfNameDescritpion",
    "vnfInfoId",
    "vmtypenameid",
    "vmWorkLoadType",
    "vmTypeNameDescription",
    "clusterDescription",
    "clusterId",
    "interVmType",
    "intraVmType",
    "nsxt",
  ];
  const instanceRenderList = [
    "vnfVmInstanceId",
    "opCoDescritpion",
    "locationName",
    "noOfVmsPerType",
    "noOfVnfInstances",
    "numa",
    "opCoId",
    "shortLocation",
    "shortLocationId",
    "socket",
  ];
  const capacityRenderList = [
    "backupRequired",
    "dataDisk",
    "eastWestBoundBandWidth",
    "financialYear",
    "iopsLoading",
    "iopsRunning",
    "northDouthBoundBandWidth",
    "osDisk",
    "otherRequirements",
    "probIngRequired",
    "ramPerVm",
    "rxTxCpuCount",
    "vcpuPerVm",
    "vmWorkLoadDistribution",
    "vnfVmCapacityId",
  ];
  const infoFilteredList = props?.renderGrid.filter((item) =>
    infoRenderList.includes(item.propertyName)
  );
  const instanceFilteredList = props?.renderGrid?.filter((item) =>
    instanceRenderList.includes(item.propertyName)
  );
  const capacityFilteredList = props.renderGrid.filter((item) =>
    capacityRenderList.includes(item.propertyName)
  );

  const instanceDelete = (type, id) => {
    console.log("instanceDelete", type, id);
  };
  const capacityDelete = (type, id) => {
    console.log("capacityDelete", type, id);
  };

  return (
    <>
      <ModalConfirm data={confirm} showHyperLink={false} />
      {/* <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
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
                    onClick={() => props.action.Edit(dropdownStates["rowId"])}
                  >
                    Edit
                  </Dropdown.Item>
                  <Dropdown.Item
                    onClick={() => {
                      props.action.Delete(
                        "infoId",
                        dropdownStates["rowId"],
                        data && data.length > 1 ? true : false
                      );
                    }}
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
                <th
                  style={{
                    minWidth: "120px",
                    padding: "10px",
                    fontSize: "15px",
                  }}
                >
                  View Instance
                </th>
                {infoFilteredList
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
                      false
                    )
                  )}
                {infoFilteredList && infoFilteredList.length ? (
                  <th className="customWidth"></th>
                ) : (
                  ""
                )}
              </tr>
            </thead>
            <tbody>
              {data?.map((item, index) => {
                const isExpanded = expandedRows.includes(item.vnfInfoId);
                const visibleColumns = infoFilteredList
                  .filter((x) => x.show)
                  .sort((a, b) => a.order - b.order);
                const rowClass = index % 2 !== 0 ? "row-even" : "row-odd";
                return (
                  <React.Fragment key={index + "-" + item.vnfInfoId}>
                    <tr
                      className={`dati ${rowClass}`}
                      key={index + "-" + item.vnfInfoId}
                      onDoubleClick={(e) =>
                        toggleDropdown(item.vnfInfoId, item, e)
                      }
                    >
                      <td
                        style={{
                          width: "50px",
                          textAlign: "center",
                          padding: "10px",
                        }}
                        key={`colapse-${item.vnfInfoId}`}
                        ref={(ref) => thRefss(ref, 0)}
                        className={`${rowClass}`}
                        onClick={() => handleToggleExpand(item.vnfInfoId)}
                        aria-label="View Instance"
                      >
                        <Tooltip title="View Instance" placement="top">
                          <span style={{ cursor: "pointer" }}>
                            {isExpanded ? (
                              <FaChevronDown />
                            ) : (
                              <FaChevronRight />
                            )}
                          </span>
                        </Tooltip>
                      </td>

                      {infoFilteredList
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
                            i + 1,
                            null,
                            null
                          )
                        )}
                      <td className={`actions ${rowClass}`}>
                        {!readonly && (
                          <div className="d-inline mx-2 cursor-pointer">
                            <img
                              className="dropdown_trigger"
                              src={require("../../img/options_dots.png")}
                              style={{ cursor: "pointer", padding: "10px" }}
                              onClick={(e) => {
                                toggleDropdown(item.vnfInfoId, item, e);
                              }}
                            />
                          </div>
                        )}
                      </td>
                    </tr>
                    {isExpanded && (
                      <tr className="collapsibleRow">
                        <td
                          colSpan={visibleColumns.length + 2}
                          style={{
                            padding: 0,
                          }}
                        >
                          <div
                            style={{
                              height: "24rem",
                              overflowY: "auto",
                              backgroundColor: "#e9ecef",
                              padding: "1rem",
                              justifyItems: "left",
                              width: "100%",
                            }}
                          >
                            <VBOMInstanceGrid
                              data={item?.["_instanceDtoGrid"]}
                              renderGrid={instanceFilteredList ?? []}
                              capacityRenderGrid={capacityFilteredList ?? []}
                              action={{
                                instanceDelete: (type, id, alowDelete) =>
                                  props.action.Delete(type, id, alowDelete),
                                capacityDelete: (type, id, alowDelete) =>
                                  props.action.Delete(type, id, alowDelete),
                              }}
                            ></VBOMInstanceGrid>
                          </div>
                        </td>
                      </tr>
                    )}
                  </React.Fragment>
                );
              })}
            </tbody>
          </table>
        </div>
      </div> */}
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
                    onClick={() => props.action.Edit(dropdownStates["rowId"])}
                  >
                    Edit
                  </Dropdown.Item>
                  <Dropdown.Item
                    onClick={() => {
                      props.action.Delete(
                        "infoId",
                        dropdownStates["rowId"],
                        data && data.length > 1 ? true : false
                      );
                    }}
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
                {infoFilteredList
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
                      false
                    )
                  )}
                {infoFilteredList && infoFilteredList.length ? (
                  <th className="customWidth"></th>
                ) : (
                  ""
                )}
              </tr>
            </thead>
            <tbody>
              {data?.map((item, index) => {
                const isExpanded = expandedRows.includes(item.vnfInfoId);
                const visibleColumns = infoFilteredList
                  .filter((x) => x.show)
                  .sort((a, b) => a.order - b.order);
                const rowClass = index % 2 !== 0 ? "row-even" : "row-odd";
                const activeRow =
                  item?.vnfInfoId === props.infoId ? true : false;
                return (
                  <React.Fragment key={index + "-" + item.vnfInfoId}>
                    <tr
                      className={`dati ${rowClass}  ${
                        activeRow ? "activeStateRow" : ""
                      }`}
                      key={index + "-" + item.vnfInfoId}
                      onClick={(e) =>
                        props.action.onInfoIdChange(item.vnfInfoId)
                      }
                      onDoubleClick={(e) =>
                        toggleDropdown(item.vnfInfoId, item, e)
                      }
                    >
                      {infoFilteredList
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
                            null,
                            null
                          )
                        )}
                      <td className={`actions ${rowClass}`}>
                        {!readonly && (
                          <div className="d-inline mx-2 cursor-pointer">
                            <img
                              className="dropdown_trigger"
                              src={require("../../img/options_dots.png")}
                              style={{
                                cursor: "pointer",
                                padding: "10px",
                              }}
                              onClick={(e) => {
                                toggleDropdown(item.vnfInfoId, item, e);
                              }}
                            />
                          </div>
                        )}
                      </td>
                    </tr>
                  </React.Fragment>
                );
              })}
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
};

export default VBOMInfoGrid;
