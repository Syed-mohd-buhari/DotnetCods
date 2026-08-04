import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import {
  GEt_USERS_LOGGING_LEVEL,
  UserLoggingLevelApi,
} from "../../../Business/UsersLogLevelsBusiness";
import { FileResult, ReturnFile } from "../../../Model/Common";
import { GetUsersLoggingLevels } from "../../../Model/UsersLoggingLevels";
import { NotifyType } from "../../Reducer/NotificationReducer";

import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetCreatePageForUsersLoggingLevels(data?: any) {
  let api = new UserLoggingLevelApi();
  setLoader("ADD", "GetCreatePageForUsersLoggingLevels");
  let result = await ApiCallWithErrorHandling<Promise<GetUsersLoggingLevels>>(
    () => api.GetCreatePageForUsersLoggingLevels(data)
  );
  let rtn = result as GetUsersLoggingLevels;

  rootStore.dispatch({ type: GEt_USERS_LOGGING_LEVEL, payload: rtn });
  setLoader("REMOVE", "GetCreatePageForUsersLoggingLevels");
  return rtn;
}

export async function GetLogLevelsById(id: number) {
  setLoader("ADD", "GetLogLevelsById");

  let api = new UserLoggingLevelApi();
  let result = await ApiCallWithErrorHandling<
    Promise<{ data: string; info: string; warning: boolean }>
  >(() => api.getLogLevelById(id));
  let rtn = result as { data: string; info: string; warning: boolean };
  setLoader("REMOVE", "GetLogLevelsById");
  return rtn;
}

export async function UserLoggingLevelCreate(body) {
  setLoader("ADD", "GetMajorHardwareBuildCreateResource");

  let api = new UserLoggingLevelApi();
  let rtn = await ApiCallWithErrorHandling<
    Promise<{ info: string; warning: boolean }>
  >(() => api.userLoggingLevelCreate(body));

  setLoader("REMOVE", "GetMajorHardwareBuildCreateResource");

  return rtn?.info;
}

export async function UserDownloadLogs(queryFilter?: any) {
  setLoader("ADD", "UserDownloadLogs");

  try {
    let api = new UserLoggingLevelApi();
    let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
      api.userDwnloadLogs(queryFilter ?? {})
    );

    console.log("res => ", res);

    let fileNameBase = res?.FileName.split(";")[1] ?? "";
    var index = fileNameBase?.indexOf('"') + 1;
    var lastIndex = fileNameBase?.indexOf('"', index);
    let fileName = fileNameBase?.substring(index, lastIndex);
    let result = res?.File.then((x) => {
      return { file: x, fileName: fileName } as FileResult;
    });
    setLoader("REMOVE", "GetDesignApectReport");

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }

  setLoader("REMOVE", "UserDownloadLogs");
}
