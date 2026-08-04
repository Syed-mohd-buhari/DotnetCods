import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ServiceBoundaryApi } from "../../../../Business/LookUp/ServiceBoundaryBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteServiceBoundary(id: number) {
  setLoader("ADD", "deleteServiceBoundary");
  let api = new ServiceBoundaryApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.serviceBoundaryDelete(id)
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
  rootStore.dispatch({ type: "DELETE_SERVICE_BOUNDARY", payload: rtn });
  setLoader("REMOVE", "deleteServiceBoundary");
  return rtn;
}

export async function DeleteDeepServiceBoundary(id: number) {
  let api = new ServiceBoundaryApi();
  setLoader("ADD", "DeleteDeepServiceBoundary");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.serviceBoundaryDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_SERVICE_BOUNDARY", payload: rtn });
  setLoader("REMOVE", "DeleteDeepServiceBoundary");
  return rtn;
}

export async function GetRelatedRecordsServiceBoundary(id: number) {
  let api = new ServiceBoundaryApi();
  setLoader("ADD", "GetRelatedRecordsServiceBoundary");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.serviceBoundaryGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsServiceBoundary");
  return rtn;
}
