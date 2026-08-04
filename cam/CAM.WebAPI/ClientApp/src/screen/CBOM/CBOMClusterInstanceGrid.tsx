import React, {
  MouseEvent,
  SetStateAction,
  useCallback,
  useEffect,
  useMemo,
  useRef,
  useState,
} from "react";
import { Dropdown } from "react-bootstrap";
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
import { calculateBodyWidths } from "../../Utils/gridFunction";
import { CBOMInstanceDtoGrid, CBOMQueryObjectGrid } from "../../Model/CBOM";
import { GetFilterColumnCBOMInstanceCapacity } from "../../Redux/Action/CBOM/CBOMGridAction";
import VBOMInstanceGrid from "./CBOMInstanceGrid";
import { calculateWidths } from "../../Components/TableCrud/TableCrudTH";

interface Props {
  action: {
    Delete(type: string, id: number | undefined, alowDelete: boolean): any;
    Edit(id: number | undefined): any;
    onInstanceIdChange(id: number | undefined): any;
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
  };
  data?: CBOMInstanceDtoGrid[] | undefined;
  pagination?: CBOMQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
  infoId?: number | null;
  instanceId?: number | null;
}

/**
 * NOTE: original file used 'firstIndex, secondIndex, thirdIndex'
 * in calculateBodyWidths. To preserve behavior I left them as module-level
 * variables (could be converted to refs if you prefer).
 */
let firstIndex: number | undefined,
  secondIndex: number | undefined,
  thirdIndex: number | undefined;

const CBOMClusterInstanceGrid: React.FC<Props> = (props) => {
  const { readonly } = useAuth();

  // local UI state
  const [data, setData] = useState<CBOMInstanceDtoGrid[] | undefined>(
    props.data
  );
  const [confirm, setConfirm] = useState<DataModalConfirm>(stateConfirm);
  const [renderGridState, setRenderGridState] = useState<
    RenderDetail[] | undefined
  >(undefined);

  // filter hook
  const getFiltersData = (state: RootState) =>
    state.CBOMClusterInstanceCapacityGridReducer.filter;
  const filterData = useSelector(getFiltersData);

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
  } = useFilterTableCrud<CBOMQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumnCBOMInstanceCapacity,
    props.pagination
  );

  // refs for th elements
  const thRefs = useRef<Array<HTMLTableCellElement | null>>([]);
  const THRefs = useRef<HTMLTableCellElement | null>(null);

  // dropdown state: only one open at a time; store rowId + item
  const [dropdownState, setDropdownState] = useState<{
    rowId?: number | null;
    item?: CBOMInstanceDtoGrid | null;
    left?: number;
    top?: number;
  }>({});

  const dropdownRef = useRef<HTMLDivElement | null>(null);

  // Keep data in sync when props.data changes
  useEffect(() => {
    setData(props.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, [props.data]);

  // Prepare renderGridState only once when props.renderGrid changes
  useEffect(() => {
    const instanceRenderList = new Set([
      "cnfPodInfoId",
      "daemonSetPod",
      "functionStandardId",
      "functionStandardName",
      "podTypeName",
      "podroleDescription",
      "interPodRules",
      "intraPodRules",
      "isEnhancedHa",
      "isPersistanceStorageFlag",
      "isProdhPaEnable",
      "podRoleDescription",
      "podRoleDescriptionId",
      "podTypeInfoName",
      "podTypeQos",
      "priorityName",
    ]);

    const instanceFilteredList = props.renderGrid.map((item) =>
      instanceRenderList.has(item.propertyName)
        ? { ...item, show: item.show ?? true }
        : { ...item, show: false }
    );

    setRenderGridState(instanceFilteredList);
  }, [props.renderGrid]);

  // th ref setter (stable reference)
  const thRefSetter = useCallback(
    (ref: HTMLTableCellElement | null, index: number) => {
      thRefs.current[index] = ref;
      calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
    },
    []
  );

  // calculate widths on props change (matching original behavior)
  useEffect(() => {
    calculateWidths(THRefs);
  }, [props]);

  // Toggle dropdown: memoized to avoid inline function recreation
  const toggleDropdown = useCallback(
    (
      rowId: number | null | undefined,
      item: CBOMInstanceDtoGrid | null | undefined,
      e: MouseEvent
    ) => {
      e.stopPropagation();
      const left = (e as any).clientX + window.scrollX;
      const top = (e as any).clientY + window.scrollY;

      const windowWidth = window.innerWidth + window.scrollX;
      const windowHeight = window.innerHeight + window.scrollY;

      const maxLeft = windowWidth - 400;
      const maxTop = windowHeight - 200;

      setDropdownState({
        rowId: rowId ?? null,
        item: item ?? null,
        left: Math.min(left, maxLeft),
        top: Math.min(top, maxTop),
      });
    },
    []
  );

  // Close dropdown when clicking outside (preserve original special-case for IMG.dropdown_trigger)
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      const target = event.target as HTMLElement | null;
      if (dropdownRef.current) {
        if (
          target &&
          target.tagName === "IMG" &&
          target.classList.contains("dropdown_trigger")
        ) {
          // leave open if the trigger itself was clicked (original behavior)
          return;
        }
        setDropdownState({});
      }
    };

    document.addEventListener("click", handleClickOutside as any);
    return () =>
      document.removeEventListener("click", handleClickOutside as any);
  }, []);

  // Simple Cancel modal helper (memoized)
  const Cancel = useCallback((msg: string) => {
    setConfirm({
      title: "Warning!",
      message: msg,
      cancelText: "Ok",
      item: 0,
      isOpen: true,
      actions: {
        cancel: () => setConfirm(stateConfirm),
      },
    });
  }, []);

  // Derived visible columns (memoized)
  const visibleColumns = useMemo(() => {
    return (renderGridState ?? [])
      .filter((x) => x.show)
      .sort((a, b) => (a.order ?? 0) - (b.order ?? 0));
  }, [renderGridState]);

  // helper used in thead mapping to keep SelectFilterType invocation stable-ish
  const thAction = useMemo(
    () => ({
      checkFilter: checkFilterinValue,
      settingVisibility: setIsVisibleFiltriString,
      resetFilter: resetFilter,
    }),
    [checkFilterinValue, setIsVisibleFiltriString, resetFilter]
  );

  const actionFilterCK = useMemo(
    () => ({
      closeAll,
      updateCount,
      getFiltriAttivi,
      orderBy,
      getFilters,
    }),
    [closeAll, updateCount, getFiltriAttivi, orderBy, getFilters]
  );

  const actionFilterDate = useMemo(
    () => ({ closeAll, setDateToChildren, orderBy }),
    [closeAll, setDateToChildren, orderBy]
  );

  const thActionDate = useMemo(
    () => ({
      checkFilter: checkFilterDateinValue,
      settingVisibility: setIsVisibleFiltriString,
      resetFilter: resetFilterDate,
    }),
    [checkFilterDateinValue, setIsVisibleFiltriString, resetFilterDate]
  );

  // Row click to set instance Id - keep as stable callback
  const onRowClick = useCallback(
    (id?: number | null) => {
      id !== null && props.action.onInstanceIdChange(id);
    },
    [props.action]
  );

  // Edit action used by dropdown
  const onEditFromDropdown = useCallback(() => {
    if (dropdownState.rowId != null) {
      props.action.Edit(dropdownState.rowId);
      setDropdownState({});
    }
  }, [dropdownState.rowId, props.action]);

  return (
    <>
      <ModalConfirm data={confirm} showHyperLink={false} />
      <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
        <div className="mx-0 px-0 flex-row" style={{ position: "relative" }}>
          {/* Dropdown positioned absolutely, only shown when dropdownState.rowId exists */}
          {!readonly && dropdownState.rowId != null && (
            <Dropdown
              className="d-inline mx-2"
              show={Boolean(dropdownState.rowId)}
              ref={dropdownRef as any}
              style={
                dropdownState.rowId
                  ? {
                      position: "absolute",
                      top: `${(dropdownState.top ?? 0) - 170}px`,
                      right: `260px`,
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
                  dropdownState.rowId ? "dropdown-menu show" : "dropdown-menu"
                }`}
              >
                <Dropdown.Item onClick={onEditFromDropdown}>Edit</Dropdown.Item>
              </div>
            </Dropdown>
          )}

          <table
            className="table-responsive overRightHeightInstanceTable"
            tabIndex={-1}
            style={{ maxHeight: "23rem" }}
          >
            <thead>
              <tr className="intestazione">
                {visibleColumns.map((item, i) =>
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
                    false,
                    undefined,
                    undefined,
                    undefined,
                    false
                  )
                )}
                <th className="customWidth"></th>
              </tr>
            </thead>

            <tbody>
              {data?.map((item, index) => {
                const activeRow = item?.cnfPodInfoId === props.instanceId;
                const rowClass = index % 2 !== 0 ? "row-even" : "row-odd";

                return (
                  <React.Fragment key={`${index}-${item.cnfPodInfoId}`}>
                    <tr
                      className={`dati ${rowClass} ${
                        activeRow ? "activeStateRow" : ""
                      }`}
                      key={index + "-" + item.cnfPodInfoId}
                      onClick={() => onRowClick(item.cnfPodInfoId)}
                      onDoubleClick={(e) =>
                        toggleDropdown(props.infoId ?? 0, item, e)
                      }
                    >
                      {visibleColumns.map((td, i) =>
                        SelectGridType(
                          (item as any)[td.propertyName],
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
                              style={{ cursor: "pointer", padding: "10px" }}
                              onClick={(e) =>
                                toggleDropdown(
                                  props.infoId ?? null,
                                  item,
                                  e as any
                                )
                              }
                              alt="options"
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

export default CBOMClusterInstanceGrid;
