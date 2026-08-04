import React, { SetStateAction, useEffect, useState, useRef } from "react";
import "../../Css/App.css";
import "../../Css/index.css";
import "../../Css/NetworkElement.css";
import { formatTimeLocal, lowerFirstLetter } from "../../Hook/Common";
import { useSelector } from "react-redux";
import { RootState } from "../../Redux/Store/rootStore";
import {
  ReportSoftwareQueryObjectGrid,
  ReportSoftwareDtoGrid,
} from "../../Model/Report/ReportSoftwareModel";
import { GetFilterColumReportSoftware } from "../../Redux/Action/Report/ReportSoftwareGridAction";
import { useFilterTableCrud } from "../../Hook/useFilterTableCrud";
import { QueryObjectGrid, RenderDetail } from "../../Model/Common";
import TH from "../../Components/TableCrud/TableCrudTH";
import {
  SelectFilterType,
  SelectGridType,
} from "../../Hook/CommonRenderGrid/GridRender";
import FilterMenuCheckboxOrderLess from "../../Components/FilterMenuCheckboxOrderSearchLess";
import GridLink from "./GridLink";
import { useAuth } from "../../Hook/useAuth";
import { calculateBodyWidths } from "../../Utils/gridFunction";

interface Props {
  action: {
    Filter(obj: SetStateAction<QueryObjectGrid>): any;
  };
  data: ReportSoftwareDtoGrid[] | undefined;
  isHistorical?: boolean;
  pagination: ReportSoftwareQueryObjectGrid | undefined;
  renderGrid: RenderDetail[];
}

let firstIndex, secondIndex, thirdIndex;
const Software: React.FC<Props> = (props) => {
  const [data, setData] = useState<ReportSoftwareDtoGrid[] | undefined>([]);
  const [isHistoricalFlag, setIsHistoricalFlag] = useState<boolean>(false);
  const { readonly, isPermesso } = useAuth();
  const getFiltersData = (state: RootState) =>
    state.reportSoftwareGridReducer.filter;
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
  } = useFilterTableCrud<ReportSoftwareQueryObjectGrid>(
    props.action.Filter,
    GetFilterColumReportSoftware,
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
    setIsHistoricalFlag(props?.isHistorical ?? false);
    calculateBodyWidths(thRefs, firstIndex, secondIndex, thirdIndex);
  }, []);

  useEffect(() => {
    setData(props?.data);
    setIsHistoricalFlag(props?.isHistorical ?? false);
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

  return (
    <div className="listaApparatiContainer mt-3 row mx-0 col-12 p-0 mb-3">
      <div className="col-12 mx-0 px-0 flex-row table-container">
        <table className="table-responsive table-thead-sticky" tabIndex={-1}>
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
                      hideFilter={isHistoricalFlag}
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
                      isHistoricalFlag ?? undefined,
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
                <div
                  className="divFilter reports customWidth"
                  style={{ minWidth: "100px" }}
                ></div>
              </th>
            </tr>
          </thead>
          <tbody>
            {data?.map((item, index) => (
              <tr className="dati" key={`SW${index}`}>
                {props.renderGrid
                  .sort((a, b) => a.order - b.order)
                  .filter((x) => x.show)
                  .map((td, i) =>
                    td.propertyName === "localMarket" &&
                    !item.archived &&
                    !readonly &&
                    !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmengineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item.localMarket}
                        />
                      </td>
                    ) : td.propertyName === "descriptionOfPlannedAction" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmengineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "plannedActivities",
                              prevPage: "generatelcmdb",
                              idDetail: item.plannedActivityId,
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "verticalEngineeringTeam" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "systemtype",
                            search: "id=" + item.systemTypeId,
                            state: {
                              id: item.systemTypeId,
                              tab: "systemtype",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item.verticalEngineeringTeam}
                        />
                      </td>
                    ) : td.propertyName === "verticalSubDomain" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "systemtype",
                            search: "id=" + item.systemTypeId,
                            state: {
                              id: item.systemTypeId,
                              tab: "systemtype",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item.verticalSubDomain}
                        />
                      </td>
                    ) : td.propertyName === "engineeringContactPoint" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmengineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "operationsContactPoint" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="  customMaxWidth"
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmengineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "assetCategory" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "systemtype",
                            search: "id=" + item.systemTypeId,
                            state: {
                              id: item.systemTypeId,
                              tab: "systemtype",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item.assetCategory}
                        />
                      </td>
                    ) : td.propertyName === "assetDescription" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "designcomponentfamily",
                            search: "id=" + item.designComponentFamilyId,
                            state: {
                              id: item.designComponentFamilyId,
                              tab: "",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "assetClass" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati"
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "systemtype",
                            search: "id=" + item.systemTypeId,
                            state: {
                              id: item.systemTypeId,
                              tab: "systemtype",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item.assetClass}
                        />
                      </td>
                    ) : td.propertyName === "assetType" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "systemtype",
                            search: "id=" + item.systemTypeId,
                            state: {
                              id: item.systemTypeId,
                              tab: "systemtype",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item.assetType}
                        />
                      </td>
                    ) : td.propertyName === "assetVirtualized" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "majorhardware",
                            search: "id=" + item.majorHardwareBuildId,
                            state: {
                              id: item.majorHardwareBuildId,
                              tab: "majorhardware",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item.assetVirtualized}
                        />
                      </td>
                    ) : td.propertyName === "productImportance" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className=" "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmengineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item.productImportance}
                        />
                      </td>
                    ) : (td.propertyName === "vendor" ||
                        td.propertyName ===
                          "extendedSupportOptionOfferedByVendor") &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "majorsoftware",
                            search: "id=" + item.majorSoftwareBuildId,
                            state: {
                              id: item.majorSoftwareBuildId,
                              tab: "MajorSoftwareBuild",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item.vendor}
                        />
                      </td>
                    ) : td.propertyName === "hardwareModel" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "majorhardware",
                            search: "id=" + item.majorHardwareBuildId,
                            state: {
                              id: item.majorHardwareBuildId,
                              tab: "",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item.hardwareModel}
                        />
                      </td>
                    ) : td.propertyName === "softwareRelease" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "majorsoftware",
                            search: "id=" + item.majorSoftwareBuildId,
                            state: {
                              id: item.majorSoftwareBuildId,
                              tab: "MajorSoftwareBuild",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item.softwareRelease}
                        />
                      </td>
                    ) : td.propertyName ===
                        "vendorEndOfVulnerabilitySecuritySupportDateValue" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmengineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={
                            item.vendorEndOfVulnerabilitySecuritySupportDateValue
                          }
                        />
                      </td>
                    ) : td.propertyName === "operationsMaintenanceContract" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className=" "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmengineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item.operationsMaintenanceContract}
                        />
                      </td>
                    ) : td.propertyName === "vendorEndOfMaintenanceDateValue" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "majorsoftware",
                            search: "id=" + item.majorSoftwareBuildId,
                            state: {
                              id: item.majorSoftwareBuildId,
                              tab: "MajorSoftwareBuild",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={
                            item.eomStatus === 0
                              ? "NOT ANNOUNCED"
                              : item.eomStatus === 1
                              ? ""
                              : formatTimeLocal(item.vendorEndOfMaintenanceDate)
                          }
                          // title={formatTimeLocal(
                          //   item.vendorEndOfMaintenanceDate
                          // )}
                        />
                      </td>
                    ) : td.propertyName === "bundleBudget" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmEngineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "plannedActivities",
                              prevPage: "generatelcmdb",
                              idDetail: item.plannedActivityId,
                            },
                          }}
                          title={item[td.propertyName]?.toString()}
                        />
                      </td>
                    ) : td.propertyName === "budgetEstimated" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmEngineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "plannedActivities",
                              prevPage: "generatelcmdb",
                              idDetail: item.plannedActivityId,
                            },
                          }}
                          title={item[td.propertyName]?.toString()}
                        />
                      </td>
                    ) : td.propertyName === "assetServiceFunctionality" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "designcomponentfamily",
                            search: "id=" + item.designComponentFamilyId,
                            state: {
                              id: item.designComponentFamilyId,
                              tab: "",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "platform" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "systemtype",
                            search: "id=" + item.systemTypeId,
                            state: {
                              id: item.systemTypeId,
                              tab: "systemtype",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "lcmStatusEng" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "systemtype",
                            search: "id=" + item.systemTypeId,
                            state: {
                              id: item.systemTypeId,
                              tab: "systemtype",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "lcmStatusOps" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "systemtype",
                            search: "id=" + item.systemTypeId,
                            state: {
                              id: item.systemTypeId,
                              tab: "systemtype",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "opsMaintenanceConractEnd" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmEngineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={formatTimeLocal(item[td.propertyName])}
                        />
                      </td>
                    ) : (td.propertyName === "engRiskEvaluation" ||
                        td.propertyName === "engRiskEvaluationNotes" ||
                        td.propertyName === "opsRiskEvaluation" ||
                        td.propertyName === "opsRiskEvaluationNotes") &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmEngineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "plannedActivities",
                              prevPage: "generatelcmdb",
                              idDetail: item.plannedActivityId,
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "descriptionOfPlannedAction" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "plannedActivities",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "plannedActivities",
                              prevPage: "generatelcmdb",
                              idDetail: item.plannedActivityId,
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "plannedHardwareModel" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmEngineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "plannedActivities",
                              prevPage: "generatelcmdb",
                              idDetail: item.plannedActivityId,
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "projectStatus" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className=" "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmEngineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "plannedActivities",
                              prevPage: "generatelcmdb",
                              idDetail: item.plannedActivityId,
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "projectEndDateValue" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmEngineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "plannedActivities",
                              prevPage: "generatelcmdb",
                              idDetail: item.plannedActivityId,
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "trackingNumberProjectName" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmEngineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "plannedActivities",
                              prevPage: "generatelcmdb",
                              idDetail: item.plannedActivityId,
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "notes" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmEngineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "plannedActivities",
                              prevPage: "generatelcmdb",
                              idDetail: item.plannedActivityId,
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName === "plannedSoftwareRelease" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmEngineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "plannedActivities",
                              prevPage: "generatelcmdb",
                              idDetail: item.plannedActivityId,
                            },
                          }}
                          title={item[td.propertyName]}
                        />
                      </td>
                    ) : td.propertyName ===
                        "assetOutofScopeForReportingPurposes" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className=" "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        {item[td.propertyName]?.toString()}
                      </td>
                    ) : td.propertyName === "numberOfNodes" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmEngineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item[td.propertyName]?.toString()}
                        />
                      </td>
                    ) : td.propertyName === "numberOfNodesInLab" &&
                      !item.archived &&
                      !readonly &&
                      !isHistoricalFlag ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
                        ref={(ref) => {
                          if (i === 0) thRefs.current[0] = ref;
                          else if (i <= 2) thRefs.current[i] = ref;
                        }}
                      >
                        <GridLink
                          to={{
                            pathname: "lcmEngineering",
                            search: "id=" + item.lcmEngineeringId,
                            state: {
                              id: item.lcmEngineeringId,
                              tab: "",
                              prevPage: "generatelcmdb",
                            },
                          }}
                          title={item[td.propertyName]?.toString()}
                        />
                      </td>
                    ) : td.propertyName === "managedByGdc" ? (
                      <td
                        className="dati "
                        key={`SW${index}${i}`}
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

                <td className="  " style={{ minWidth: "100px" }}></td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default Software;
