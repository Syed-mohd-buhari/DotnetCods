import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFClusterApi } from "../../../../Business/LookUp/CNFClusterBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteCNFCluster(id: number) {
  setLoader("ADD", "DeleteCNFCluster");
  let api = new CNFClusterApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFClusterDelete(id)
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
  rootStore.dispatch({ type: "DELETE_CNFCLUSTER", payload: rtn });
  setLoader("REMOVE", "DeleteCNFCluster");
  return rtn;
}

export async function DeleteDeepCNFCluster(id: number) {
  let api = new CNFClusterApi();
  setLoader("ADD", "DeleteDeepCNFCluster");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFClusterDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_CNFCLUSTER", payload: rtn });
  setLoader("REMOVE", "DeleteDeepCNFCluster");
  return rtn;
}
