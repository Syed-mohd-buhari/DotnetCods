import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import {
  GET_GENERIC_REPORT,
  GenericReportApi,
} from "../../../Business/GenericReportBusiness";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { ResultDto } from "../../../Model/CommonModels";
import {
  GET_GRID_GENERIC_REPORT,
  GET_GRID_AGGREGATED_GENERIC_REPORT,
  GET_GRID_DISAGGREGATED_GENERIC_REPORT,
  GET_GRID_GENERIC_PREVIEW_REPORT,
  GenericReportGrid,
  GenericAggregatedReportGrid,
  GenericDisAggregatedReportGrid,
  GenericReportQueryObjectGrid,
  GetGenericReportResponse,
  QueryResultDtoOfGenericReportDtoGrid,
  GenericPreviewReportGrid,
  QueryResultDtoOfPreviewGenericReportDtoGrid,
  GET_FILTER_GENERIC_REPORT,
  GET_FILTER_AGGREGATED_GENERIC_REPORT,
  GET_FILTER_DISAGGREGATED_GENERIC_REPORT,
  GenericPreviewReportQueryObjectGrid,
  GET_FILTER_GENERIC_PREVIEW_REPORT,
  GenericViewReportQueryObjectGrid,
} from "../../../Model/GenericReport";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetGenericReport(data?: any) {
  let api = new GenericReportApi();
  setLoader("ADD", "GetGenericReport");
  let result = await ApiCallWithErrorHandling<
    Promise<GetGenericReportResponse>
  >(() => api.GetGenericReport(data));
  let rtn = result as GetGenericReportResponse;

  rootStore.dispatch({ type: GET_GENERIC_REPORT, payload: rtn });
  setLoader("REMOVE", "GetGenericReport");
  return rtn;
}

export async function GetGenericReportGrid(
  queryFilter?: GenericReportQueryObjectGrid
) {
  setLoader("ADD", "GetGenericReportGrid");
  let api = new GenericReportApi();

  let result: QueryResultDtoOfGenericReportDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfGenericReportDtoGrid>
    >(() => api.GetGenericGrid(queryFilter ?? {}));

    let rtn = {
      GenericReportGridResult: result,
      filter: null,
    } as GenericReportGrid;

    rootStore.dispatch({ type: GET_GRID_GENERIC_REPORT, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_GENERIC_REPORT,
      payload: {
        GenericReportGridResult: null,
        filter: null,
      } as GenericReportGrid,
    });
  }
  setLoader("REMOVE", "GetGenericReportGrid");
}

export async function GetGenericAggregatedReportGrid(
  queryFilter?: GenericViewReportQueryObjectGrid
) {
  setLoader("ADD", "GetGenericAggregatedReportGrid");
  let api = new GenericReportApi();

  let result: QueryResultDtoOfPreviewGenericReportDtoGrid | null | undefined;
  try {
    result = await api.genericReportPreview(queryFilter ?? {});
    let rtn = {
      GenericAggregatedReportGridResult: result,
      filter: null,
    } as GenericAggregatedReportGrid;

    rootStore.dispatch({
      type: GET_GRID_AGGREGATED_GENERIC_REPORT,
      payload: rtn,
    });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_AGGREGATED_GENERIC_REPORT,
      payload: {
        GenericAggregatedReportGridResult: null,
        filter: null,
      } as GenericAggregatedReportGrid,
    });
  }
  setLoader("REMOVE", "GetGenericAggregatedReportGrid");
}

export async function GetGenericDisAggregatedReportGrid(
  queryFilter?: GenericViewReportQueryObjectGrid
) {
  setLoader("ADD", "GetGenericDisAggregatedReportGrid");
  let api = new GenericReportApi();

  let result: QueryResultDtoOfPreviewGenericReportDtoGrid | null | undefined;
  try {
    result = await api.genericReportPreview(queryFilter ?? {});
    let rtn = {
      GenericDisAggregatedReportGridResult: result,
      filter: null,
    } as GenericDisAggregatedReportGrid;

    rootStore.dispatch({
      type: GET_GRID_DISAGGREGATED_GENERIC_REPORT,
      payload: rtn,
    });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_DISAGGREGATED_GENERIC_REPORT,
      payload: {
        GenericDisAggregatedReportGridResult: null,
        filter: null,
      } as GenericDisAggregatedReportGrid,
    });
  }
  setLoader("REMOVE", "GetGenericDisAggregatedReportGrid");
  return result;
}

export async function PreviewReport(
  queryFilter?: GenericViewReportQueryObjectGrid,
  apiType?: string
) {
  setLoader("ADD", "PreviewReport");
  let api = new GenericReportApi();

  let result: QueryResultDtoOfPreviewGenericReportDtoGrid | null | undefined;
  try {
    result = await api.genericReportPreview(queryFilter ?? {}, apiType);
    let rtn = {
      GenericPreviewReportGridResult: result,
      filter: null,
    } as GenericPreviewReportGrid;

    rootStore.dispatch({ type: GET_GRID_GENERIC_PREVIEW_REPORT, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_GENERIC_PREVIEW_REPORT,
      payload: {
        GenericPreviewReportGridResult: null,
        filter: null,
      } as GenericPreviewReportGrid,
    });
  }
  setLoader("REMOVE", "PreviewReport");
  return result;
}

export async function CreateGenericReport(body) {
  setLoader("ADD", "CreateGenericReport");

  let api = new GenericReportApi();
  let rtn = await ApiCallWithErrorHandling<
    Promise<{ info: string; warning: boolean }>
  >(() => api.genericReportCreate(body));

  setLoader("REMOVE", "CreateGenericReport");
  return rtn;
}

export async function UpdateReportStatus(reportId: number) {
  setLoader("ADD", "UpdateReportStatus");
  let api = new GenericReportApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.genericReportUpdateStatus(reportId)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "Failed to Submit",
      notifyType:
        result !== undefined && result.warning === true
          ? NotifyType.success
          : NotifyType.error,
    })
  );
  setLoader("REMOVE", "UpdateReportStatus");
  return rtn;
}

export async function CloneReportStatus(payload: any) {
  setLoader("ADD", "CloneReport");
  let api = new GenericReportApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.genericReportClone(payload)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "Failed to Submit",
      notifyType:
        result !== undefined && result.warning === true
          ? NotifyType.success
          : NotifyType.error,
    })
  );
  setLoader("REMOVE", "CloneReport");
  return rtn;
}

export async function DeleteReport(reportId: number) {
  setLoader("ADD", "DeleteReport");
  let api = new GenericReportApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.genericReportDelete(reportId)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "Failed to Submit",
      notifyType:
        result !== undefined && result.warning === true
          ? NotifyType.success
          : NotifyType.error,
    })
  );
  setLoader("REMOVE", "DeleteReport");
  return rtn;
}

export async function GetFilterColumGenericReport(
  columName: string,
  columValue: string,
  queryFilter?: GenericReportQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new GenericReportApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.genericReportGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    GenericReportGridResult: null,
  } as GenericReportGrid;
  rootStore.dispatch({ type: GET_FILTER_GENERIC_REPORT, payload: rtn });
}

export async function GetFilterColumGenericAggregatedReport(
  columName: string,
  columValue: string,
  queryFilter?: GenericReportQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new GenericReportApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.genericReportGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    GenericAggregatedReportGridResult: null,
  } as GenericAggregatedReportGrid;
  rootStore.dispatch({
    type: GET_FILTER_AGGREGATED_GENERIC_REPORT,
    payload: rtn,
  });
}
export async function GetFilterColumGenericDisAggregatedReport(
  columName: string,
  columValue: string,
  queryFilter?: GenericReportQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new GenericReportApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.genericReportGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    GenericDisAggregatedReportGridResult: null,
  } as GenericDisAggregatedReportGrid;
  rootStore.dispatch({
    type: GET_FILTER_DISAGGREGATED_GENERIC_REPORT,
    payload: rtn,
  });
}
export async function GetFilterColumGenericPreviewReport(
  columName: string,
  columValue: string,
  queryFilter?: GenericPreviewReportQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new GenericReportApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.genericPreviewReportGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    GenericPreviewReportGridResult: null,
  } as GenericPreviewReportGrid;
  rootStore.dispatch({ type: GET_FILTER_GENERIC_PREVIEW_REPORT, payload: rtn });
}

export async function DownloadGenericReport(data: any) {
  setLoader("ADD", "DownloadGenericReport");
  let api = new GenericReportApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.DownloadGenericReport(data)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "DownloadGenericReport");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "DownloadGenericReport");
}

export async function DownloadGenericCSVReport(data: any) {
  setLoader("ADD", "DownloadGenericCSVReport");
  let api = new GenericReportApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.DownloadGenericCSVReport(data)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "DownloadGenericCSVReport");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "DownloadGenericCSVReport");
}
