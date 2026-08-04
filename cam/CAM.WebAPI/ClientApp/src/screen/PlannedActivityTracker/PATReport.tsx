import React, { SetStateAction, useEffect, useState, useRef } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import TH from "../../Components/TableCrud/TableCrudTH";
import FilterMenuCheckboxOrderLess from "../../Components/FilterMenuCheckboxOrderSearchLess";

import {
  PATReportDto,
  ReportPATQueryObjectGrid,
} from "../../Model/Report/PlannedActivityTrackerModel";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { QueryObjectGrid } from "../../Model/Common";
import { GetFilterColumReportPAT } from "../../Redux/Action/Report/PlannedActivityTrackerAction";
import { lowerFirstLetter } from "../../Hook/Common";
import { RootState } from "../../Redux/Store/rootStore";
import { useSelector } from "react-redux";
//commit
interface Props {
  action: {
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
  };
  data: PATReportDto | undefined | null;
  //viewReport: boolean;
  pagination: ReportPATQueryObjectGrid | undefined;
}

const PATReportTable: React.FC<Props> = (props) => {
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
        const computedStylesR3 = window
          .getComputedStyle(tableHeadElements[2])
          ?.getPropertyValue("left");

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
        } else if (columnIndex === 3 && computedStylesR3) {
          // Apply styles for the third column
          bodyElements.forEach((td, index) => {
            if (index % 2 === 0) {
              td.classList.add("table_tr_bg");
            } else {
              td.classList.add("table_tr_even_bg");
            }
            td.style.left = `${computedStylesR3}`;
            td.style.position = "sticky";
            td.style.zIndex = "1";
            td.style.borderRight = "1px solid #ccc";
            td.style.boxShadow = "5px 0 5px rgba(0,0,0,0.1)";
            td.style.filter = "drop-shadow(2px 0px 0px rgba(0,0,0,0.1))";
          });
        }
      }
    });
  };

  const [months, setMonths] = useState<string[]>([]);

  const getFiltersData = (state: RootState) =>
    state.reportPATGridReducer.filter;
  let filterData = useSelector(getFiltersData);

  useEffect(() => {
    calculateBodyWidths(thRefs);
  }, []);
  useEffect(() => {
    manageMonths();
  }, [props.data]);

  const manageMonths = () => {
    if (props?.data?.items?.length) {
      setMonths(props?.data?.items[0].months);
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
  } = useFilterTableCrud<ReportPATQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumReportPAT,
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
      case "red":
        return "bgGridRed";
      case "yellow":
        return "bgGridYellow";
      case "blue":
        return "bgGridBlue";
      case "green":
        return "bgGridGreen";
      case "white":
        return "bgGridWhite";
      case "gray":
        return "bgGridGray";
      case "black":
        return "bgGridBlack";
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
                  propertyName={"opCoId"}
                  action={thAction}
                  key={"opCoId"}
                  isVisibleFiltriString={isVisibleFiltriString}
                  setupDuplicates={false}
                  freezeHeader={true}
                >
                  <FilterMenuCheckboxOrderLess
                    orderAscending={props.pagination?.isSortAscending}
                    propertyInOrder={props.pagination?.sortBy}
                    filterData={filterData ?? undefined}
                    overrideProperty=""
                    property={lowerFirstLetter("opCoId")}
                    FiltriAttivi={filtriAttivi?.["opCoId"]}
                    count={count}
                    action={actionFilterCK}
                  />
                </TH>
                <TH
                  spanClassName={""}
                  propertyName={"dcfId"}
                  action={thAction}
                  key={"dcfId"}
                  isVisibleFiltriString={isVisibleFiltriString}
                  setupDuplicates={false}
                  freezeHeader={true}
                >
                  <FilterMenuCheckboxOrderLess
                    orderAscending={props.pagination?.isSortAscending}
                    propertyInOrder={props.pagination?.sortBy}
                    filterData={filterData ?? undefined}
                    overrideProperty=""
                    property={lowerFirstLetter("dcfId")}
                    FiltriAttivi={filtriAttivi?.["dcfId"]}
                    count={count}
                    action={actionFilterCK}
                  />
                </TH>

                <TH
                  spanClassName={""}
                  propertyName={"verticalNameId"}
                  action={thAction}
                  key={"verticalNameId"}
                  isVisibleFiltriString={isVisibleFiltriString}
                  setupDuplicates={false}
                  overridePropertyName="Vertical Name"
                  freezeHeader={true}
                >
                  <FilterMenuCheckboxOrderLess
                    orderAscending={props.pagination?.isSortAscending}
                    propertyInOrder={props.pagination?.sortBy}
                    filterData={filterData ?? undefined}
                    overrideProperty=""
                    property={lowerFirstLetter("verticalNameId")}
                    FiltriAttivi={filtriAttivi?.["verticalNameId"]}
                    count={count}
                    action={actionFilterCK}
                  />
                </TH>
                {months.map((month, idx) => (
                  <th
                    key={idx}
                    className="customVolteKPIHead text-left pl-2"
                    style={{ fontSize: "13px", padding: "0 20px" }}
                  >
                    <div className="h-100 d-flex align-items-center divFilter">
                      <span>{month}</span>
                    </div>
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {props.data?.items?.map((item, i) => (
                <tr className="dati" key={i}>
                  <td
                    ref={(ref) => {
                      if (i === 0) thRefs.current[0] = ref;
                      else if (i <= 2) thRefs.current[i] = ref;
                    }}
                  >
                    {item.opcoName === "" ||
                    item.opcoName === undefined ||
                    item.opcoName === null
                      ? "---"
                      : item.opcoName}
                  </td>
                  <td
                    ref={(ref) => {
                      if (i === 0) thRefs.current[0] = ref;
                      else if (i <= 2) thRefs.current[i] = ref;
                    }}
                  >
                    <p
                      dangerouslySetInnerHTML={{
                        __html: item.dcfName === "" ? "---" : item.dcfName,
                      }}
                    ></p>
                  </td>
                  <td
                    ref={(ref) => {
                      if (i === 0) thRefs.current[0] = ref;
                      else if (i <= 2) thRefs.current[i] = ref;
                    }}
                  >
                    {item.verticalName === "" ||
                    item.verticalName === undefined ||
                    item.verticalName === null
                      ? "---"
                      : item.verticalName}
                  </td>
                  {item.releases &&
                    item.releases.map((rlse, index) => (
                      <td
                        key={index}
                        className={`${getColorCode(
                          rlse?.split("|")[1]?.trim()
                        )} borderRightStyle`}
                      >
                        {rlse?.split("|")[0] ?? rlse}
                      </td>
                    ))}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default PATReportTable;
