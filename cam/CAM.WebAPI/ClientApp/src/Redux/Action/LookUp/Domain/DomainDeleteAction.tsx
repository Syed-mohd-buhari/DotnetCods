import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SystemNamesApi } from "../../../../Business/LookUp/DomainBuisness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function deleteSystemName(id: number) {
  setLoader("ADD", "deleteSystemName");
  let api = new SystemNamesApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.systemNamesDelete(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "DELETE_SYSTEM_NAME", payload: rtn });
  setLoader("REMOVE", "deleteSystemName");
  return rtn;
}

export async function deleteDeepSystemName(id: number) {
  setLoader("ADD", "deleteDeepSystemName");
  let api = new SystemNamesApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.systemNamesDelete(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "DELETE_SYSTEM_NAME", payload: rtn });
  setLoader("REMOVE", "deleteDeepSystemName");
  return rtn;
}

export async function getRelatedRecordsSystemName(id: number) {
  setLoader("ADD", "getRelatedRecordsSystemName");
  let api = new SystemNamesApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.systemNamesGetRelatedRecords(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  if (result?.warning)
    rootStore.dispatch(
      setNotification({ message: result?.info ?? "", notifyType: NotifyType.error })
    );
  setLoader("REMOVE", "getRelatedRecordsSystemName");
  return rtn;
}
