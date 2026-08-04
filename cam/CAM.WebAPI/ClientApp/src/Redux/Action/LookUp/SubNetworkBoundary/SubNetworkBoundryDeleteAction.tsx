import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SubNetworkBoundaryApi } from "../../../../Business/LookUp/SubnetworkBoundryBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteSubNetworkBoundary(id: number) {
  setLoader("ADD", "deleteSubNetworkBoundary");
  let api = new SubNetworkBoundaryApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.subNetworkBoundaryDelete(id)
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
  rootStore.dispatch({ type: "DELETE_SUBNETWORK_BOUNDARY", payload: rtn });
  setLoader("REMOVE", "deleteSubNetworkBoundary");
  return rtn;
}

export async function DeleteDeepSubNetworkBoundary(id: number) {
  let api = new SubNetworkBoundaryApi();
  setLoader("ADD", "DeleteDeepSubNetworkBoundary");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.subNetworkBoundaryDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_SUBNETWORK_BOUNDARY", payload: rtn });
  setLoader("REMOVE", "DeleteDeepSubNetworkBoundary");
  return rtn;
}

export async function GetRelatedRecordsSubNetworkBoundary(id: number) {
  let api = new SubNetworkBoundaryApi();
  setLoader("ADD", "GetRelatedRecordsSubNetworkBoundary");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.subNetworkBoundaryGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsSubNetworkBoundary");
  return rtn;
}
