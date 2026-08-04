import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VNFClusterNameApi } from "../../../../Business/LookUp/VNFClusterNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteVNFClusterName(id: number) {
  setLoader("ADD", "DeleteVNFClusterName");
  let api = new VNFClusterNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VNFClusterNameDelete(id)
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
  rootStore.dispatch({ type: "DELETE_VNFCLUSTERNAME", payload: rtn });
  setLoader("REMOVE", "DeleteVNFClusterName");
  return rtn;
}

export async function DeleteDeepVNFClusterName(id: number) {
  let api = new VNFClusterNameApi();
  setLoader("ADD", "DeleteDeepVNFClusterName");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VNFClusterNameDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_VNFCLUSTERNAME", payload: rtn });
  setLoader("REMOVE", "DeleteDeepVNFClusterName");
  return rtn;
}
