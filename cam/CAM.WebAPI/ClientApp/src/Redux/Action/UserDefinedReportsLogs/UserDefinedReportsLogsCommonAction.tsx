import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { UserDefinedReportsLogsApi } from "../../../Business/UserDefinedReportsLogsBusiness";
import { ReturnFile, FileResult } from "../../../Model/Common";
import { ResultDto } from "../../../Model/CommonModels";

import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import {
  GET_FILTER_USER_DEFINED_REPORTS_LOGS,
  GET_GRID_USER_DEFINED_REPORTS_LOGS,
  QueryResultDtoOfUserDefinedReportsLogsDtoGrid,
  UserDefinedReportsLogsGrid,
  UserDefinedReportsLogsQueryObjectGrid,
} from "../../../Model/UserDefinedReportsLogs";

export async function GetUserDefinedReportsLogsGrid(
  queryFilter?: UserDefinedReportsLogsQueryObjectGrid
) {
  setLoader("ADD", "GetUserDefinedReportsLogsGrid");
  let api = new UserDefinedReportsLogsApi();

  let result: QueryResultDtoOfUserDefinedReportsLogsDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfUserDefinedReportsLogsDtoGrid>
    >(() => api.GetUserDefinedReportsLogsGrid(queryFilter ?? {}));

    let rtn = {
      UserDefinedReportsLogsGridResult: result,
      filter: null,
    } as unknown as UserDefinedReportsLogsGrid;

    rootStore.dispatch({
      type: GET_GRID_USER_DEFINED_REPORTS_LOGS,
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
      type: GET_GRID_USER_DEFINED_REPORTS_LOGS,
      payload: {
        UserDefinedReportsLogsGridResult: null,
        filter: null,
      } as unknown as UserDefinedReportsLogsGrid,
    });
  }
  setLoader("REMOVE", "GetUserDefinedReportsLogsGrid");
}

export async function GetFilterColumnUserDefinedReportsLogs(
  columName: string,
  columValue: string,
  queryFilter?: UserDefinedReportsLogsQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new UserDefinedReportsLogsApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.UserDefinedReportsLogsGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    UserDefinedReportsLogsGridResult: null,
  } as unknown as UserDefinedReportsLogsGrid;
  rootStore.dispatch({
    type: GET_FILTER_USER_DEFINED_REPORTS_LOGS,
    payload: rtn,
  });
}

export async function DownloadUserDefinedReportsLogs(data: any) {
  setLoader("ADD", "DownloadUserDefinedReportsLogs");
  let api = new UserDefinedReportsLogsApi();
  let res: ReturnFile | undefined;
  try {
    res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.DownloadUserDefinedReportsLogs(data)
    );
    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "DownloadUserDefinedReportsLogs");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "DownloadUserDefinedReportsLogs");
}
