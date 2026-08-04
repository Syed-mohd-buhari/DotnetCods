import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SupportedServiceApi } from "../../../../Business/LookUp/SupportedServiceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteSupportedService(id: number) {
  setLoader("ADD", "deleteSupportedService");
  let api = new SupportedServiceApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.supportedServiceDelete(id)
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
  rootStore.dispatch({ type: "DELETE_SUPPORTED_SERVICE", payload: rtn });
  setLoader("REMOVE", "deleteSupportedService");
  return rtn;
}

export async function DeleteDeepSupportedService(id: number) {
  let api = new SupportedServiceApi();
  setLoader("ADD", "DeleteDeepSupportedService");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.supportedServiceDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_SUPPORTED_SERVICE", payload: rtn });
  setLoader("REMOVE", "DeleteDeepSupportedService");
  return rtn;
}

export async function GetRelatedRecordsSupportedService(id: number) {
  let api = new SupportedServiceApi();
  setLoader("ADD", "GetRelatedRecordsSupportedService");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.supportedServiceGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsSupportedService");
  return rtn;
}
