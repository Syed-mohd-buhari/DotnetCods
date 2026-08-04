import React, { SetStateAction, useEffect, useState, useRef } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import TH from "../../Components/TableCrud/TableCrudTH";
import FilterMenuCheckboxOrderLess from "../../Components/FilterMenuCheckboxOrderSearchLess";

import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { QueryObjectGrid } from "../../Model/Common";
import { lowerFirstLetter } from "../../Hook/Common";
import { RootState } from "../../Redux/Store/rootStore";
import { useSelector } from "react-redux";
import {
  AssetPivotByLocationDto,
  AssetPivotByLocationQueryObjectGrid,
} from "../../Model/Report/AssetPivotByLocationModel";
import { GetFilterColumnAssetPivotByLocation } from "../../Redux/Action/Report/AssetPivotByLocationAction";
//commit
interface Props {
  action: {
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
  };
  data: AssetPivotByLocationDto | undefined | null;
  //viewReport: boolean;
  pagination: AssetPivotByLocationQueryObjectGrid | undefined;
}

const AssetPivotByLocationTable: React.FC<Props> = (props) => {
  const calculateBodyWidths = (thRefs) => {
    thRefs.current.forEach((ref, index) => {
      if (ref) {
        const tableHeadElements =
          document.querySelectorAll<HTMLTableDataCellElement>(
            `thead > tr > th`
          );
        const computedStylesR2 = window
          .getComputedStyle(tableHeadElements[1])
          ?.getPropertyValue("left");
        // const computedStylesR3 = window
        //   .getComputedStyle(tableHeadElements[index + 1])
        //   ?.getPropertyValue("left");

        const bodyElements =
          document.querySelectorAll<HTMLTableDataCellElement>(
            `tbody > tr > td:nth-child(${index + 1})`
          );
        const columnIndex = index + 1;

        if (columnIndex === 1) {
          // Apply styles for the first column
          bodyElements.forEach((td, index) => {
            if (index % 2 === 0) {
              td.classList.add("table_tr_bg");
            } else {
              td.classList.add("table_tr_even_bg");
            }
            td.style.left = `${0}px`;
            td.style.position = "sticky";
            td.style.zIndex = "1";
          });
        } else if (columnIndex === 2 && computedStylesR2) {
          // Apply styles for the second column
          bodyElements.forEach((td, index) => {
            if (index % 2 === 0) {
              td.classList.add("table_tr_bg");
            } else {
              td.classList.add("table_tr_even_bg");
            }
            td.style.left = `${computedStylesR2}`;
            td.style.position = "sticky";
            td.style.zIndex = "1";
          });
        } else {
          // Apply styles for the third column if (computedStylesR3 && columnIndex > 2)
          bodyElements.forEach((td, index) => {
            if (index % 2 === 0) {
              td.classList.add("table_tr_bg");
            } else {
              td.classList.add("table_tr_even_bg");
            }
            //td.style.left = `${computedStylesR3}`;
            // td.style.position = "sticky";
            // td.style.zIndex = "1";
            // td.style.borderRight = "1px solid #ccc";
            // td.style.boxShadow = "5px 0 5px rgba(0,0,0,0.1)";
            // td.style.filter = "drop-shadow(2px 0px 0px rgba(0,0,0,0.1))";
          });
        }
      }
    });
  };

  const [locations, setLocations] = useState(Array<{ [key: string]: string }>);

  const getFiltersData = (state: RootState) =>
    state.AssetPivotByLocationGridReducer.filter;
  let filterData = useSelector(getFiltersData);

  useEffect(() => {
    calculateBodyWidths(thRefs);
  }, []);
  useEffect(() => {
    managelocations();
  }, [props.data]);

  const managelocations = () => {
    if (props?.data?.items?.length) {
      setLocations(props?.data?.items[0].locations);
      setTimeout(() => {
        calculateBodyWidths(thRefs);
      }, 1000);
    }
  };

  const {
    filtriAttivi,
    checkFilterinValue,
    setIsVisibleFiltriString,
    resetFilter,
    count,
    closeAll,
    updateCount,
    getFiltriAttivi,
    setFiltriAttivi,
    orderBy,
    getFilters,
    isVisibleFiltriString,
  } = useFilterTableCrud<AssetPivotByLocationQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumnAssetPivotByLocation,
    props.pagination
  );

  // useEffect(() => {
  //   console.log("rerender filter =>", props.pagination);
  //   setFiltriAttivi(props.pagination);
  // }, [filtriAttivi]);

  let thRefs = useRef<Array<HTMLTableCellElement | null>>([
    null,
    null,
    null,
    null,
  ]); // Refs for th elements

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
  const getColorCode = (colorCode) => {
    switch (colorCode) {
      case "apricot":
        return "bgGridApricot";

      default:
        return "";
    }
  };

  return (
    <div className="listaApparatiContainer mt-3 mx-0 col-12 p-0 justify-content-center">
      <div className="col-12 mx-0 px-0">
        <div className="mx-0 px-0 flex-row table-container">
          <table
            className="table-responsive table-thead-sticky"
            style={{ minHeight: "in" }}
          >
            <thead>
              <tr
                className="intestazione"
                style={{ backgroundColor: "#f5f5f5" }}
              >
                <TH
                  spanClassName={""}
                  propertyName={"paImplementaionYear"}
                  action={thAction}
                  key={"paImplementaionYear"}
                  isVisibleFiltriString={isVisibleFiltriString}
                  setupDuplicates={false}
                  freezeHeader={true}
                >
                  <FilterMenuCheckboxOrderLess
                    orderAscending={props.pagination?.isSortAscending}
                    propertyInOrder={props.pagination?.sortBy}
                    filterData={filterData ?? undefined}
                    overrideProperty=""
                    property={lowerFirstLetter("paImplementaionYear")}
                    FiltriAttivi={filtriAttivi?.["paImplementaionYear"]}
                    count={count}
                    action={actionFilterCK}
                  />
                </TH>
                <TH
                  spanClassName={""}
                  propertyName={"designComponent"}
                  action={thAction}
                  key={"designComponent"}
                  isVisibleFiltriString={isVisibleFiltriString}
                  setupDuplicates={false}
                  freezeHeader={true}
                >
                  <FilterMenuCheckboxOrderLess
                    orderAscending={props.pagination?.isSortAscending}
                    propertyInOrder={props.pagination?.sortBy}
                    filterData={filterData ?? undefined}
                    overrideProperty=""
                    property={lowerFirstLetter("designComponent")}
                    FiltriAttivi={filtriAttivi?.["designComponent"]}
                    count={count}
                    action={actionFilterCK}
                  />
                </TH>
                <TH
                  spanClassName={""}
                  propertyName={"verticalName"}
                  action={thAction}
                  key={"verticalName"}
                  isVisibleFiltriString={isVisibleFiltriString}
                  setupDuplicates={false}
                  freezeHeader={true}
                >
                  <FilterMenuCheckboxOrderLess
                    orderAscending={props.pagination?.isSortAscending}
                    propertyInOrder={props.pagination?.sortBy}
                    filterData={filterData ?? undefined}
                    overrideProperty=""
                    property={lowerFirstLetter("verticalName")}
                    FiltriAttivi={filtriAttivi?.["verticalName"]}
                    count={count}
                    action={actionFilterCK}
                  />
                </TH>
                {locations?.map((location, idx) => (
                  <th
                    key={idx}
                    className="text-left pl-2"
                    style={{ fontSize: "13px", padding: "0 20px" }}
                  >
                    <div className="h-100 d-flex align-items-center divFilter">
                      <span>{location.key.split("|")[0]?.trim()}</span>
                    </div>
                  </th>
                ))}
                <th
                  className="text-left pl-2"
                  style={{ fontSize: "13px", padding: "0 20px" }}
                >
                  Total
                </th>
              </tr>
            </thead>
            <tbody>
              {props.data?.items?.map((item, i) => (
                <tr className="dati" key={i}>
                  <td
                    // className={`${getColorCode(
                    //   item?.paImplementaionYear?.split("|")[1]?.trim()
                    // )} borderRightStyle`}
                    ref={(ref) => {
                      if (i === 0) thRefs.current[0] = ref;
                      else if (i <= 2) thRefs.current[i] = ref;
                    }}
                  >
                    {item.paImplementaionYear === "" ||
                    item.paImplementaionYear === undefined ||
                    item.paImplementaionYear === null
                      ? "---"
                      : item.paImplementaionYear?.split("|")[0]?.trim()}
                  </td>
                  <td
                    ref={(ref) => {
                      if (i === 0) thRefs.current[0] = ref;
                      else if (i <= 2) thRefs.current[i] = ref;
                    }}
                  >
                    <p
                      dangerouslySetInnerHTML={{
                        __html:
                          item.designComponent === ""
                            ? "---"
                            : item.designComponent,
                      }}
                    ></p>
                  </td>
                  <td
                    ref={(ref) => {
                      if (i === 0) thRefs.current[0] = ref;
                      else if (i <= 2) thRefs.current[i] = ref;
                    }}
                  >
                    <p
                      dangerouslySetInnerHTML={{
                        __html:
                          item.verticalName === "" ||
                          item.verticalName === undefined ||
                          item.verticalName === null
                            ? "---"
                            : item.verticalName,
                      }}
                    ></p>
                  </td>
                  {item.locations &&
                    item.locations.map((loc, index) => (
                      <td
                        key={index}
                        className={`${getColorCode(
                          loc?.key?.split("|")[1]?.trim()
                        )} borderRightStyle`}
                      >
                        {loc.value != "" ? loc.value : "--"}
                      </td>
                    ))}
                  <td style={{ fontWeight: "bold" }}>
                    {item.total != "" ? item.total : "--"}
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

export default AssetPivotByLocationTable;
