import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { CBOMApi } from "../../../Business/CBOMBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_CBOM, RESTORE_CBOM } from "../../../Model/CBOM";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteCBOM(id: number) {
  setLoader("ADD", "DeleteCBOM");
  let api = new CBOMApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CBOMDelete(id)
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
  rootStore.dispatch({ type: DELETE_CBOM, payload: rtn });
  setLoader("REMOVE", "DeleteCBOM");
  return rtn;
}

export async function CBOMDelete(id: number, apiType?: string | undefined) {
  setLoader("ADD", "DeleteCBOM");
  let api = new CBOMApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CBOMDelete(id, apiType)
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
  rootStore.dispatch({ type: DELETE_CBOM, payload: rtn });
  setLoader("REMOVE", "DeleteCBOM");
  return rtn;
}
