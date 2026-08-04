import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ClassApi } from "../../../../Business/LookUp/ClassBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteClass(id: number) {
  setLoader("ADD", "deleteClass");
  let api = new ClassApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.ClassDelete(id)
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
  rootStore.dispatch({ type: "DELETE_CLASS", payload: rtn });
  setLoader("REMOVE", "deleteClass");
  return rtn;
}

export async function DeleteDeepClass(id: number) {
  let api = new ClassApi();
  setLoader("ADD", "DeleteDeepClass");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.ClassDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_CLASS", payload: rtn });
  setLoader("REMOVE", "DeleteDeepClass");
  return rtn;
}

export async function GetRelatedRecordsClass(id: number) {
  let api = new ClassApi();
  setLoader("ADD", "GetRelatedRecordsClass");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.ClassGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsClass");
  return rtn;
}
