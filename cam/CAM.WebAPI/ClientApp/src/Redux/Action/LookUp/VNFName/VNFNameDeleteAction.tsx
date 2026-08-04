import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VNFNameApi } from "../../../../Business/LookUp/VNFNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteVNFName(id: number) {
  setLoader("ADD", "DeleteVNFName");
  let api = new VNFNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VNFNameDelete(id)
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
  rootStore.dispatch({ type: "DELETE_VNFNAME", payload: rtn });
  setLoader("REMOVE", "DeleteVNFName");
  return rtn;
}

export async function DeleteDeepVNFName(id: number) {
  let api = new VNFNameApi();
  setLoader("ADD", "DeleteDeepVNFName");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VNFNameDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_VNFNAME", payload: rtn });
  setLoader("REMOVE", "DeleteDeepVNFName");
  return rtn;
}
