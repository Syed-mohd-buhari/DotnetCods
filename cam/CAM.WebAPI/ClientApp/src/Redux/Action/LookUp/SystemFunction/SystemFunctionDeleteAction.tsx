import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SystemFunctionApi } from "../../../../Business/LookUp/SystemFunctionBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteSystemFunction(id: number) {
  setLoader("ADD", "deleteSystemFunction");
  let api = new SystemFunctionApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.systemFunctionDelete(id)
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
  rootStore.dispatch({ type: "DELETE_SYSTEM_FUNCTION", payload: rtn });
  setLoader("REMOVE", "deleteSystemFunction");
  return rtn;
}

export async function DeleteDeepSystemFunction(id: number) {
  let api = new SystemFunctionApi();
  setLoader("ADD", "DeleteDeepSystemFunction");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.systemFunctionDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_SYSTEM_FUNCTION", payload: rtn });
  setLoader("REMOVE", "DeleteDeepSystemFunction");
  return rtn;
}

export async function GetRelatedRecordsSystemFunction(id: number) {
  let api = new SystemFunctionApi();
  setLoader("ADD", "GetRelatedRecordsSystemFunction");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.systemFunctionGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsSystemFunction");
  return rtn;
}
