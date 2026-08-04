import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VMTypeNameApi } from "../../../../Business/LookUp/VMTypeNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteVMTypeName(id: number) {
  setLoader("ADD", "DeleteVMTypeName");
  let api = new VMTypeNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VMTypeNameDelete(id)
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
  rootStore.dispatch({ type: "DELETE_VMTYPENAME", payload: rtn });
  setLoader("REMOVE", "DeleteVMTypeName");
  return rtn;
}

export async function DeleteDeepVMTypeName(id: number) {
  let api = new VMTypeNameApi();
  setLoader("ADD", "DeleteDeepVMTypeName");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VMTypeNameDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_VMTYPENAME", payload: rtn });
  setLoader("REMOVE", "DeleteDeepVMTypeName");
  return rtn;
}
