import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NetworkFunctionApi } from "../../../../Business/LookUp/NetworkFunctionBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteNetworkFunction(id: number) {
  setLoader("ADD", "deleteNetworkFunction");
  let api = new NetworkFunctionApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkFunctionDelete(id)
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
  rootStore.dispatch({ type: "DELETE_NETWORK_FUNCTION", payload: rtn });
  setLoader("REMOVE", "deleteNetworkFunction");
  return rtn;
}

export async function DeleteDeepNetworkFunction(id: number) {
  let api = new NetworkFunctionApi();
  setLoader("ADD", "DeleteDeepNetworkFunction");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkFunctionDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_NETWORK_FUNCTION", payload: rtn });
  setLoader("REMOVE", "DeleteDeepNetworkFunction");
  return rtn;
}

export async function GetRelatedRecordsNetworkFunction(id: number) {
  let api = new NetworkFunctionApi();
  setLoader("ADD", "GetRelatedRecordsNetworkFunction");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkFunctionGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsNetworkFunction");
  return rtn;
}
