import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { ExodusReportApi } from "../../../Business/Report/ExodusBuisness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import {
  ExodusDTO,
  ExodusGrid,
  ExodusReportGrid,
  GET_GRID_EXODUS_REPORT,
  GET_FILTER_EXODUS_REPORT,
  QueryResultDtoOfExodusReportDtoGrid,
} from "../../../Model/Report/Exodus";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetExodusReportGrid(queryFilter?: ExodusDTO) {
  setLoader("ADD", "GetExodusGrid");
  let result: QueryResultDtoOfExodusReportDtoGrid | null | undefined;
  let api = new ExodusReportApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfExodusReportDtoGrid>
    >(() => api.exodusGetReport(queryFilter ?? {}));

    let rtn = {
      ExodusReportGridResult: result,
      filter: null,
    } as ExodusReportGrid;
    rootStore.dispatch({ type: GET_GRID_EXODUS_REPORT, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_EXODUS_REPORT,
      payload: {
        ExodusReportGridResult: null,
        filter: null,
      } as ExodusReportGrid,
    });
  }
  setLoader("REMOVE", "GetExodusGrid");
}

export async function GetExodusReport(queryFilter?: ExodusDTO) {
  setLoader("ADD", "GetExodusReport");

  let api = new ExodusReportApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.ExodusReportExportReport(queryFilter ?? {})
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetExodusReport");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetExodusReport");
}

export async function GetFilterColumExodusReport(
  columName: string,
  columValue: string,
  queryFilter?: ExodusDTO
) {
  let api = new ExodusReportApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.exodusReportGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    ExodusReportGridResult: null,
  } as ExodusReportGrid;
  rootStore.dispatch({ type: GET_FILTER_EXODUS_REPORT, payload: rtn });

  return rtn;
}
