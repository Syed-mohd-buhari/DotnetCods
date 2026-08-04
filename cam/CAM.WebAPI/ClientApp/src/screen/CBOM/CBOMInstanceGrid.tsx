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
import { CBOMDtoGrid } from "../../Model/CBOM";
import { GetFilterColumnCBOM } from "../../Redux/Action/CBOM/CBOMGridAction";
import { FaChevronDown, FaChevronRight } from "react-icons/fa";
import { calculateWidths } from "../../Components/TableCrud/TableCrudTH";
import VBOMInstanceGrid from "./CBOMInstanceGrid";
import { Box, Button, Grid, Tooltip } from "@mui/material";

interface Props {
  action: {
    Delete(type: string, id: number | undefined, alowDelete: boolean): any;
    Edit(id: number | undefined): any;
    onInstanceIdChange(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
  };
  data: CBOMDtoGrid[] | undefined;
  pagination: OrganizationInfoQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
  infoId?: number | null;
  instanceId?: number | null;
}

let firstIndex, secondIndex, thirdIndex;
const CBOMGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<CBOMDtoGrid[] | undefined>([]);
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [renderGridState, setRenderGridState] = useState<any>();
  const { readonly, isPermesso } = useAuth();
  const getFiltersData = (state: RootState) => state.CBOMGridReducer.filter;
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
    GetFilterColumnCBOM,
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
    if (props.infoId !== null) {
      setData(
        props?.data?.filter((res) => res.cnfInfoId === props.infoId)?.[0]?.[
          "_cbomClusterInfoDtoGrid"
        ]
      );
    } else setData(props?.data?.[0]?.["_cbomClusterInfoDtoGrid"]);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, [props.data, props.infoId, props.instanceId]);

  useEffect(() => {
    const instanceRenderList = [
      "cnfClusterInfoInstanceId",
      "cnfClusterNameDescription",
      "daemonSetPod",
      "functionStandardId",
      "functionStandardName",
      "interPodRules",
      "intraPodRules",
      "isEnhancedHa",
      "isPersistanceStorageFlag",
      "isProdhPaEnable",
      "location",
      "nodePoolName",
      "opcoId",
      "opcoName",
      "podRoleDescription",
      "podRoleDescriptionId",
      "podTypeInfoName",
      "podTypeQos",
      "priority",
      "site",
      "siteId",
    ];

    const instanceFilteredList = props.renderGrid.map((item) => {
      if (!instanceRenderList.includes(item.propertyName)) {
        return { ...item, show: false };
      }
      return item;
    });
    setRenderGridState(instanceFilteredList);
  }, [props.renderGrid]);

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

  return (
    <>
      <ModalConfirm data={confirm} showHyperLink={false} />
      <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
        <div className="mx-0 px-0 flex-row" style={{ position: "relative" }}>
          <table
            className="table-responsive overRightHeight"
            tabIndex={-1}
            style={{ maxHeight: "23rem !important" }}
          >
            <thead>
              <tr className="intestazione">
                {renderGridState
                  ?.filter((x) => x.show)
                  // .sort((a, b) => a.order - b.order)
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
                      true,
                      undefined,
                      undefined,
                      undefined,
                      false
                    )
                  )}
              </tr>
            </thead>
            <tbody>
              {data?.map((item, index) => {
                const isExpanded = expandedRows.includes(
                  item.cnfClusterInfoInstanceId
                );
                const visibleColumns = renderGridState
                  .filter((x) => x.show)
                  .sort((a, b) => a.order - b.order);
                const rowClass = index % 2 !== 0 ? "row-even" : "row-odd";
                const activeRow =
                  item?.cnfClusterInfoInstanceId === props.instanceId
                    ? true
                    : false;
                return (
                  <React.Fragment
                    key={index + "-" + item.cnfClusterInfoInstanceId}
                  >
                    <tr
                      className={`dati ${rowClass}  ${
                        activeRow ? "activeStateRow" : ""
                      }`}
                      key={index + "-" + item.cnfClusterInfoInstanceId}
                      onClick={(e) =>
                        props.action.onInstanceIdChange(
                          item.cnfClusterInfoInstanceId
                        )
                      }
                    >
                      {renderGridState
                        ?.filter((x) => x.show)
                        // .sort((a, b) => a.order - b.order)
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

export default CBOMGrid;
