import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFPriorityApi } from "../../../../Business/LookUp/CNFPriorityBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteCNFPriority(id: number) {
  setLoader("ADD", "DeleteCNFPriority");
  let api = new CNFPriorityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFPriorityDelete(id)
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
  rootStore.dispatch({ type: "DELETE_CNFPRIORITY", payload: rtn });
  setLoader("REMOVE", "DeleteCNFPriority");
  return rtn;
}

export async function DeleteDeepCNFPriority(id: number) {
  let api = new CNFPriorityApi();
  setLoader("ADD", "DeleteDeepCNFPriority");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFPriorityDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_CNFPRIORITY", payload: rtn });
  setLoader("REMOVE", "DeleteDeepCNFPriority");
  return rtn;
}
