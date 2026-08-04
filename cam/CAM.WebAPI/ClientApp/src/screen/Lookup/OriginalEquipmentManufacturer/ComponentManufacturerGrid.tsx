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
import { GetFilterColumComponentManufacturer } from "../../../Redux/Action/LookUp/ComponentManufacture/ComponentManufacturerGridAction";
import {
  TipologicheQueryObjectGrid,
  TipologicaGridDto,
} from "../../../Model/LookUp/LookUpGenericModel";
import { Dropdown } from "react-bootstrap";
import ThreeDot from "../../../Components/TableCrud/ThreeDot";

interface Props {
  action: {
    onDelete(id: number | undefined): any;
    Edit(id: number | undefined): any;
    Filter(obj: SetStateAction<TipologicheQueryObjectGrid> | undefined): any;
  };
  data: TipologicaGridDto[] | undefined;
  pagination: TipologicheQueryObjectGrid | undefined;

  renderGrid: RenderDetail[];
}

const ComponentManufacturerGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<TipologicaGridDto[] | undefined>([]);
  const getFiltersData = (state: RootState) =>
    state.originalEquipmentManufacturerGridReducer.filter;
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
  } = useFilterTableCrud<TipologicheQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumComponentManufacturer,
    props.pagination
  );

  //CARICAMENTO INIZIALE
  useEffect(() => {
    setData(props.data);
  }, []);
  //UPDATE DATA
  useEffect(() => {
    setData(props?.data);
  }, [props.data]);

  //CHIAMATA AL PARENT AL CAMBIO FILTRI
  useEffect(() => {
    props.action.Filter(filtriAttivi);
  }, [filtriAttivi]);

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

  // replace threedot click functionality
  const [dropdownStates, setDropdownStates] = useState({});
  const [dropdownPosition, setDropdownPosition] = useState<{
    left: number;
    top: number;
  }>({ left: 0, top: 0 });
  const dropdownRef = useRef<HTMLDivElement>(null);
  const containerRef = useRef<HTMLTableElement>(null);

  // Function to toggle dropdown state for a specific row
  const toggleDropdown = (rowId, event) => {
    setDropdownStates(() => ({
      rowId,
    }));
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

  return (
    <div className="listaApparatiContainer row mx-0 col-12 p-0 d-flex justify-content-center">
      <div className="" style={{ position: "relative" }}>
        {dropdownStates["rowId"] && (
          <Dropdown
            className="d-inline mx-2"
            show={dropdownStates["rowId"] ? true : false}
            ref={dropdownRef}
          >
            <div
              className={`${
                dropdownStates["rowId"] ? "dropdown-menu show" : "dropdown-menu"
              }`}
              style={
                dropdownStates["rowId"]
                  ? {
                      position: "absolute",
                      top: `${dropdownPosition.top + 50}px`,
                      left: `${dropdownPosition.left + 120}px`,
                      transform: "translate(-50%, -50%)",
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
              <Dropdown.Item
                onClick={() => props.action.Edit(dropdownStates["rowId"])}
              >
                Edit
              </Dropdown.Item>
              <Dropdown.Item
                onClick={() => props.action.onDelete(dropdownStates["rowId"])}
              >
                Delete
              </Dropdown.Item>
            </div>
          </Dropdown>
        )}
        <table className="w-100 table-responsive" ref={containerRef}>
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
                    undefined
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
              data?.map((item, i) => (
                <tr
                  className="dati"
                  key={item.id}
                  style={{ position: "relative" }}
                  onDoubleClick={(e) => toggleDropdown(item.id, e)}
                >
                  {props.renderGrid
                    .sort((a, b) => a.order - b.order)
                    .filter((x) => x.show)
                    .map((td, i) =>
                      SelectGridType(
                        item[td.propertyName],
                        td.propertyName,
                        td.type
                      )
                    )}
                  <td className="actions">
                    {
                      <div className="d-inline mx-2 cursor-pointer">
                        <img
                          className="dropdown_trigger"
                          src={require("../../../img/options_dots.png")}
                          style={{ cursor: "pointer", padding: "10px" }}
                          onClick={(e) => toggleDropdown(item.componentManufacturerId, e)}
                        />
                      </div>
                    }
                  </td>
                </tr>
              ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default ComponentManufacturerGrid;
