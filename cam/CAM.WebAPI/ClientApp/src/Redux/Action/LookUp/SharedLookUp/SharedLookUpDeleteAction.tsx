import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SharedLookUpApi } from "../../../../Business/LookUp/SharedLookUpBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function deleteSharedLookUp(apiType: string, id: number) {
  setLoader("ADD", "deleteSharedLookUp");
  let api = new SharedLookUpApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.sharedLookUpDelete(apiType, id)
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
  rootStore.dispatch({ type: "DELETE_SHARED_LOOKUP", payload: rtn });
  setLoader("REMOVE", "deleteSharedLookUp");
  return rtn;
}

export async function DeleteDeepSharedLookUp(id: number, apiType?: string) {
  let api = new SharedLookUpApi();
  setLoader("ADD", "DeleteDeepSharedLookUp");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.sharedLookUpDeleteDeep(id, apiType)
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
  rootStore.dispatch({ type: "DELETE_SHARED_LOOKUP", payload: rtn });
  setLoader("REMOVE", "DeleteDeepSharedLookUp");
  return rtn;
}

export async function GetRelatedRecordsSharedLookUp(
  apiType: string,
  id: number
) {
  let api = new SharedLookUpApi();
  setLoader("ADD", "GetRelatedRecordsSharedLookUp");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.sharedLookUpGetRelatedRecords(apiType, id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  if (result?.warning)
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: NotifyType.error,
      })
    );
  setLoader("REMOVE", "GetRelatedRecordsSharedLookUp");
  return rtn;
}
