import React, { useEffect, useState, useRef } from "react";
import { RenderDetail } from "../../Model/Common";
import { useAuth } from "../../Hook/useAuth";
import { GeneralSettingsDtoGrid } from "../../Model/GeneralSettingsModal";
import { SelectGridType } from "../../Hook/CommonRenderGrid/GridRender";
import { Dropdown, Modal } from "react-bootstrap";
import { formatTimeLocal } from "../../Hook/Common";

interface Props {
  action: {
    Edit(id: number | undefined): any;
  };
  data: GeneralSettingsDtoGrid[] | undefined;
  renderGrid: RenderDetail[];
  orphanColor?: boolean;
}

const GeneralSettingsGrid: React.FC<Props> = ({
  data,
  renderGrid,
  orphanColor,
  action,
}) => {
  const [gridData, setGridData] = useState<
    GeneralSettingsDtoGrid[] | undefined
  >([]);
  const { readonly, isPermesso } = useAuth();
  const [dropdownStates, setDropdownStates] = useState({});
  const [dropdownPosition, setDropdownPosition] = useState<{
    left: number;
    top: number;
  }>({ left: 0, top: 0 });
  // Update internal state when props change
  useEffect(() => {
    setGridData(data);
  }, [data]);

  const dropdownRef = useRef<HTMLDivElement>(null);

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

  return (
    <>
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
                    onClick={() => action.Edit(dropdownStates["rowId"])}
                  >
                    Edit
                  </Dropdown.Item>
                </>
              </div>
            </Dropdown>
          )}
          <table className="table-responsive" tabIndex={-1}>
            <thead>
              <tr className="intestazione">
                <th
                  className="customVolteKPIHead text-left pl-2"
                  style={{ fontSize: "13px", padding: "0 20px" }}
                >
                  <div className="h-100 d-flex align-items-center divFilter">
                    <span>No. of records per page</span>
                  </div>
                </th>
                <th
                  className="customVolteKPIHead text-left pl-2"
                  style={{ fontSize: "13px", padding: "0 20px" }}
                >
                  <div className="h-100 d-flex align-items-center divFilter">
                    <span>Last Modified</span>
                  </div>
                </th>
                <th
                  className="customVolteKPIHead text-left pl-2"
                  style={{ fontSize: "13px", padding: "0 20px" }}
                >
                  <div className="h-100 d-flex align-items-center divFilter">
                    <span>Last Modified By</span>
                  </div>
                </th>
                <th className="customWidth"></th>

                {/* {renderGrid.map((property,idx)=>(
                  <th
                  key={idx}
                  className="customVolteKPIHead text-left pl-2"
                  style={{ fontSize: "13px", padding: "0 20px" }}
                >
                  <div className="h-100 d-flex align-items-center divFilter">
                    <span>{property.propertyName}</span>
                  </div>
                </th>
                ))
} */}
              </tr>
            </thead>
            <tbody>
              {gridData?.map((item, index) => (
                <tr className="dati" key={index + "-" + item.appSettingsId}>
                  <td>{item.settingsValue}</td>
                  <td>{formatTimeLocal(item.lastModified)}</td>
                  <td>{item.lastModifiedBy}</td>
                  <td className="actions">
                    {!readonly && (
                      <div className="d-inline mx-2 cursor-pointer">
                        <img
                          className="dropdown_trigger"
                          src={require("../../img/options_dots.png")}
                          style={{ cursor: "pointer", padding: "10px" }}
                          onClick={(e) => {
                            toggleDropdown(item.appSettingsId, item, e);
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

export default GeneralSettingsGrid;
