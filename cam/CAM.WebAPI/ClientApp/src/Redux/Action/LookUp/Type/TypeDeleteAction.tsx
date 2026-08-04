import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { TypeApi } from "../../../../Business/LookUp/TypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteType(id: number) {
  setLoader("ADD", "deleteType");
  let api = new TypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.TypeDelete(id)
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
  rootStore.dispatch({ type: "DELETE_Type", payload: rtn });
  setLoader("REMOVE", "deleteType");
  return rtn;
}

export async function DeleteDeepType(id: number) {
  let api = new TypeApi();
  setLoader("ADD", "DeleteDeepType");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.TypeDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_TYPE", payload: rtn });
  setLoader("REMOVE", "DeleteDeepType");
  return rtn;
}

export async function GetRelatedRecordsType(id: number) {
  let api = new TypeApi();
  setLoader("ADD", "GetRelatedRecordsType");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.TypeGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsType");
  return rtn;
}
