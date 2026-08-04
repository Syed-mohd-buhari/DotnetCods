import React, { SetStateAction, useEffect, useState, useRef } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { formatTimeLocal, lowerFirstLetter } from "../../Hook/Common";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import {
  PassThroughReportQueryObjectGrid,
  PassThroughReportDtoGrid,
} from "../../Model/Report/PassThroughReportExport";
import { GetFilterColumPassThroughReport } from "../../Redux/Action/Report/PassThroughReportGridAction";
import { getColor } from "../../Containers/GenerateLcmDbContainer";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { QueryObjectGrid, RenderDetail } from "../../Model/Common";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import TH from "../../Components/TableCrud/TableCrudTH";
import FilterMenuCheckboxOrderLess from "../../Components/FilterMenuCheckboxOrderSearchLess";
import { useAuth } from "./../../Hook/useAuth";
import { calculateBodyWidths } from "../../Utils/gridFunction";

interface Props {
  action: {
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
  };
  data: PassThroughReportDtoGrid[] | undefined;
  pagination: PassThroughReportQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
}

let firstIndex, secondIndex, thirdIndex;
const PassThroughReportGrid: React.FC<Props> = (props) => {
  const [data, setData] = useState<PassThroughReportDtoGrid[] | undefined>([]);
  const { readonly, isPermesso } = useAuth();
  const getFiltersData = (state: RootState) =>
    state.passThroughReportGridReducer.filter;
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
  } = useFilterTableCrud<PassThroughReportQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumPassThroughReport,
    props.pagination
  );

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

  useEffect(() => {
    setData(props?.data);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, [props.data]);

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

  const optionValue = (data: string) => {
    if (data && data !== "" && data != null) {
      try {
        let teset = "";

        var dataObj = JSON.parse(data);
        for (var key in dataObj) {
          teset += `${key} : ${dataObj[key]}  </br> `;
        }
        return teset;
      } catch (error) {
        return data;
      }
    }
  };

  return (
    <div className="listaApparatiContainer mt-3 row mx-0 col-12  p-0 mb-3">
      <div className="col-12 mx-0 px-0 flex-row table-container">
        <table className=" table-responsive table-thead-sticky" tabIndex={-1}>
          <thead>
            <tr className="intestazione">
              {props.renderGrid
                .sort((a, b) => a.order - b.order)
                .filter((x) => x.show)
                .map((item, i) =>
                  item.propertyName === "lcmStatusEng" ||
                  item.propertyName === "lcmStatusOps" ||
                  item.propertyName === "lcmStatus" ? (
                    <TH
                      spanClassName={item.colorHeader}
                      propertyName={item.propertyName}
                      action={thAction}
                      key={item.propertyName}
                      hideFilter={undefined}
                      isVisibleFiltriString={isVisibleFiltriString}
                      setupDuplicates={false}
                      freezeHeader={true}
                    >
                      <FilterMenuCheckboxOrderLess
                        orderAscending={props.pagination?.isSortAscending}
                        propertyInOrder={props.pagination?.sortBy}
                        filterData={filterData ?? undefined}
                        overrideProperty=""
                        property={lowerFirstLetter(item.propertyName)}
                        FiltriAttivi={filtriAttivi?.[item.propertyName]}
                        count={count}
                        action={actionFilterCK}
                      />
                    </TH>
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
                      item.colorHeader,
                      // isHistoricalFlag ?? undefined,
                      undefined,
                      item?.propertyName === "originalHwLcmId" ||
                        item?.propertyName === "originalSwLcmId"
                        ? "Original LCM Spreadsheet ID"
                        : undefined,
                      undefined,
                      undefined,
                      true
                    )
                  )
                )}
              <th className=" ">
                <div className="divFilter reports customWidth"></div>
              </th>
            </tr>
          </thead>
          <tbody>
            {data?.map((item, index) => (
              <tr className="dati" key={`HW${index}`}>
                {props.renderGrid
                  .sort((a, b) => a.order - b.order)
                  .filter((x) => x.show)
                  .map((td, i) =>
                    td.propertyName === "vendorEndOfMaintenanceDateValue" ? (
                      <td
                        className="dati"
                        key={`HW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        {item.eomStatus === 0
                          ? "NOT ANNOUNCED"
                          : item.eomStatus === 1
                          ? ""
                          : formatTimeLocal(item.vendorEndOfMaintenanceDate)}
                      </td>
                    ) : td.propertyName ===
                      "assetOutofScopeForReportingPurposes" ? (
                      <td
                        className="dati"
                        key={`HW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        {item[td.propertyName]?.toString()}
                      </td>
                    ) : td.propertyName === "managedByGdc" ? (
                      <td
                        className=""
                        key={`HW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        No
                      </td>
                    ) : (
                      SelectGridType(
                        item[td.propertyName],
                        td.propertyName,
                        td.type,
                        "",
                        undefined,
                        item["eomStatus"],
                        "eomStatus",
                        i,
                        thRefs,
                        thRefss
                      )
                    )
                  )}
                <td className=" " style={{ minWidth: "100px" }}></td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default PassThroughReportGrid;
