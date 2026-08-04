import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFNameApi } from "../../../../Business/LookUp/CNFNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteCNFName(id: number) {
  setLoader("ADD", "DeleteCNFName");
  let api = new CNFNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFNameDelete(id)
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
  rootStore.dispatch({ type: "DELETE_CNFNAME", payload: rtn });
  setLoader("REMOVE", "DeleteCNFName");
  return rtn;
}

export async function DeleteDeepCNFName(id: number) {
  let api = new CNFNameApi();
  setLoader("ADD", "DeleteDeepCNFName");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFNameDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_CNFNAME", payload: rtn });
  setLoader("REMOVE", "DeleteDeepCNFName");
  return rtn;
}
