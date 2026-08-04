import React, { SetStateAction, useEffect, useRef, useState } from "react";
import { Dropdown } from "react-bootstrap";
import { useSelector } from "react-redux";
import ThreeDot from "../../../Components/TableCrud/ThreeDot";
import { formatDateOnlyString } from "../../../Hook/Common";
import { useFilterTableCrud } from "../../../Hook/useFilterTableCrud";
import { RootState } from "../../../Redux/Store/rootStore";
import { GetFilterColumAssetsPlatformMigrationGrid } from "../../../Redux/Action/AssetsPlatform/AssetsDetailsGridAction";
import {
  DaAssetMigrationDto,
  DaAssetMigrationGrid,
} from "../../../Model/LookUp/AssetMigrationModels";
import { TipologicheQueryObjectGridRule } from "../../../Model/LookUp/LookUpGenericModel";
import { RenderDetail } from "../../../Model/Common";
import {
  SelectFilterType,
  SelectGridType,
} from "../../../Hook/CommonRenderGrid/GridRender";
import { calculateBodyWidths } from "../../../Utils/gridFunction";

interface Props {
  data: DaAssetMigrationGrid[];
  pagination: TipologicheQueryObjectGridRule;
  action: {
    Filter(obj: SetStateAction<DaAssetMigrationGrid>): any;
    setIsFiltriAttivati(value: boolean): any;
    Edit(uniqueId: string): void;
    onDelete(uniqueId: string): void;
    isEnable: boolean;
  };
  renderGrid: RenderDetail[];
}

let firstIndex, secondIndex, thirdIndex;
const AssetsDetailsGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<DaAssetMigrationGrid[] | undefined>([]);

  const getFiltersData = (state: RootState) =>
    state.daassetsMigrationGridReducer.filter;

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
  } = useFilterTableCrud<DaAssetMigrationGrid>(
    props.action.Filter,
    GetFilterColumAssetsPlatformMigrationGrid,
    props.pagination
  );

  const [dropdownStates, setDropdownStates] = useState<any>({});
  const [dropdownPosition, setDropdownPosition] = useState<{
    left: number;
    top: number;
  }>({ left: 0, top: 0 });
  const dropdownRef = useRef<HTMLDivElement>(null);

  const toggleDropdown = (rowId: string, event: React.MouseEvent) => {
    setDropdownStates({ rowId });
    setDropdownPosition({
      left: event.clientX,
      top: event.clientY,
    });
  };

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current) {
        const target = event.target as HTMLElement;
        if (
          target.tagName === "IMG" &&
          target.classList.contains("dropdown_trigger")
        ) {
          return;
        }
        setDropdownStates({});
      }
    };
    document.body.addEventListener("click", handleClickOutside);
    return () => {
      document.body.removeEventListener("click", handleClickOutside);
    };
  }, []);

  useEffect(() => {
    setData(props.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, []);

  useEffect(() => {
    setData(props?.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, [props.data]);

  useEffect(() => {
    props.action.setIsFiltriAttivati(isFiltriAttivati);
  }, [isFiltriAttivati]);

  let thRefs = useRef<Array<HTMLTableCellElement | null>>([
    null,
    null,
    null,
    null,
  ]);

  const thRefss = (ref, index) => {
    if (index === 0) {
      thRefs.current[0] = ref;
      calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
    } else if (index !== 0 && index <= 3) {
      thRefs.current[index] = ref;
      calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
    }
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

  return (
    <div className="listaApparatiContainer mx-0 col-12 p-0 justify-content-center">
      <div
        className="mx-0 px-0 flex-row table-container"
        style={{ position: "relative" }}
      >
        {props.action.isEnable && dropdownStates["rowId"] && (
          <Dropdown
            className="d-inline mx-2"
            show={true}
            ref={dropdownRef}
            style={{
              position: "fixed",
              top: `${dropdownPosition.top}px`,
              left: `${dropdownPosition.left}px`,
              zIndex: 9999,
            }}
          >
            <div className="dropdown-menu show">
              <Dropdown.Item
                onClick={() => {
                  props.action.Edit(dropdownStates["rowId"]);
                  setDropdownStates({});
                }}
              >
                Edit
              </Dropdown.Item>
              <Dropdown.Item
                onClick={() => {
                  props.action.onDelete(dropdownStates["rowId"]);
                  setDropdownStates({});
                }}
              >
                Delete
              </Dropdown.Item>
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
                    false,
                    item.propertyName === "newDeploymentStatus"
                      ? "New Deployment Status"
                      : undefined,
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
            {data?.map((item, index) => {
              return (
                <tr
                  className={`dati`}
                  key={item.uniqueId}
                  onDoubleClick={(e) => {
                    toggleDropdown(item.uniqueId ?? "", e);
                  }}
                  style={{ cursor: "pointer" }}
                >
                  {props.renderGrid
                    .filter((x) => x.show)
                    .sort((a, b) => a.order - b.order)
                    .map((td, i) => {
                      let displayValue = item[td.propertyName];
                      // if (
                      //   td.propertyName === "newDeploymentStatus" &&
                      //   (!displayValue || displayValue.length === 0)
                      // ) {
                      //   displayValue = item.oldDeploymentStatus;
                      // }
                      return SelectGridType(
                        displayValue,
                        td.propertyName,
                        td.type,
                        "",
                        undefined,
                        undefined,
                        undefined,
                        i,
                        thRefs,
                        thRefss
                      );
                    })}
                  {props.action.isEnable && (
                    <td className="actions">
                      <div className="d-inline mx-2 cursor-pointer">
                        <img
                          className="dropdown_trigger"
                          src={require("../../../img/options_dots.png")}
                          style={{ cursor: "pointer", padding: "10px" }}
                          onClick={(e) => {
                            toggleDropdown(item.uniqueId ?? "", e);
                          }}
                          alt="options"
                        />
                      </div>
                    </td>
                  )}
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default AssetsDetailsGrid;
