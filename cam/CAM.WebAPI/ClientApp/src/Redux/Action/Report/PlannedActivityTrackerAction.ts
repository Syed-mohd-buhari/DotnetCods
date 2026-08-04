import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import {
  ReportPATGrid,
  PATReportDto,
  GET_GRID_REPORT_PAT,
  ReportPATQueryObjectGrid,
  GET_FILTER_REPORT_PAT,
  SWOEM_MODEL,
} from "../../../Model/Report/PlannedActivityTrackerModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { PATReport } from "../../../Business/PlannedActivityReportTrackerBusiness";
import { rootStore } from "../../Store/rootStore";
import { setNotification } from "../NotificationAction";
import setLoader from "../LoaderAction";
import { FileResult, ReturnFile } from "../../../Model/Common";
// import { useDispatch } from 'react-redux'

export async function GetReportPATGrid(queryFilter?: ReportPATQueryObjectGrid) {
  setLoader("ADD", "GetReportPATGrid");
  let result: PATReportDto | null | undefined;
  let api = new PATReport();

  try {
    result = await ApiCallWithErrorHandling<Promise<PATReportDto>>(() =>
      api.patGetAll(queryFilter)
    );

    let rtn = {
      ReportPATGridResult: result,
      filter: null,
    } as ReportPATGrid;
    rootStore.dispatch({ type: GET_GRID_REPORT_PAT, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    var rtn = {
      ReportPATGridResult: null,
      filter: null,
    } as ReportPATGrid;
    rootStore.dispatch({ type: GET_GRID_REPORT_PAT, payload: rtn });
  }

  setLoader("REMOVE", "GetReportPATGrid");
}

export async function GetReportPATAllExports(
  queryFilter?: ReportPATQueryObjectGrid
) {
  setLoader("ADD", "GetReportPATAllExports");
  let api = new PATReport();

  let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
    api.PATxportReport(
      queryFilter?.buildConstruction,
      queryFilter?.opCoId,
      queryFilter?.dcfId,
      queryFilter?.vendorIds,
      queryFilter?.isEosDateEnable,
      queryFilter?.sortBy,
      queryFilter?.isSortAscending,
      queryFilter?.page,
      queryFilter?.pageSize,
      queryFilter?.principalId,
      queryFilter?.deleted,
      queryFilter?.orphan,
      queryFilter?.lastModified
    )
  );
  let fileNameBase = res?.FileName.split(";")[1] ?? "";
  var index = fileNameBase?.indexOf('"') + 1;
  var lastIndex = fileNameBase?.indexOf('"', index);
  let fileName = fileNameBase?.substring(index, lastIndex);
  let result = res?.File.then((x) => {
    return { file: x, fileName: fileName } as FileResult;
  });
  setLoader("REMOVE", "GetReportPATAllExports");
  return result;
}

export async function GetFilterColumReportPAT(
  columName: string,
  columValue: string,
  queryFilter?: ReportPATQueryObjectGrid
) {
  let api = new PATReport();

  console.log("querty filer => ", queryFilter);

  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.reportPATGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  var rtn = {
    filter: result,
    ReportPATGridResult: null,
  } as ReportPATGrid;
  rootStore.dispatch({ type: GET_FILTER_REPORT_PAT, payload: rtn });
}

export async function GetSWOem() {
  setLoader("ADD", "GetSWOem");
  let api = new PATReport();

  let rtn = await ApiCallWithErrorHandling<Promise<SWOEM_MODEL>>(() =>
    api.getSWOem()
  );

  let result = {
    SWOemResources: rtn,
  } as SWOEM_MODEL;

  setLoader("REMOVE", "GetSWOem");
  return result;
}

export async function GetVendorsOfPredefinedFilter(id: number) {
  setLoader("ADD", "GetVendorsOfPredefinedFilter");
  let api = new PATReport();
  let rtn = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.getVendorsOfPredefinedFilter(id)
  );

  // let result = {
  //   SWOemResources: rtn,
  // } as SWOEM_MODEL;

  let result = rtn;

  setLoader("REMOVE", "GetVendorsOfPredefinedFilter");
  return result;
}
