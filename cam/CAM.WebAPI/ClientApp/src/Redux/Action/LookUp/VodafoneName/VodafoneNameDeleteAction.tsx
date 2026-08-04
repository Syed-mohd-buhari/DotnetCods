import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VodafoneNameApi } from "../../../../Business/LookUp/VodafoneNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteVodafoneName(id: number) {
  let api = new VodafoneNameApi();
  setLoader("ADD", "deleteVodafoneName");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.vodafoneNameDelete(id)
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
  rootStore.dispatch({ type: "DELETE_VODAFONE_NAME", payload: rtn });
  setLoader("REMOVE", "deleteVodafoneName");
  return rtn;
}

export async function DeleteDeepVodafoneName(id: number) {
  let api = new VodafoneNameApi();
  setLoader("ADD", "DeleteDeepVodafoneName");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.vodafoneNameDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_VODAFONE_NAME", payload: rtn });
  setLoader("REMOVE", "DeleteDeepVodafoneName");
  return rtn;
}

export async function GetRelatedRecordsVodafoneName(id: number) {
  let api = new VodafoneNameApi();
  setLoader("ADD", "GetRelatedRecordsVodafoneName");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.vodafoneNameGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsVodafoneName");
  return rtn;
}
