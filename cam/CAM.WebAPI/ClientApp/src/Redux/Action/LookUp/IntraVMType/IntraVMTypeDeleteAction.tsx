import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { IntraVMTypeApi } from "../../../../Business/LookUp/IntraVMTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteIntraVMType(id: number) {
  setLoader("ADD", "DeleteIntraVMType");
  let api = new IntraVMTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.IntraVMTypeDelete(id)
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
  rootStore.dispatch({ type: "DELETE_INTRAVMTYPE", payload: rtn });
  setLoader("REMOVE", "DeleteIntraVMType");
  return rtn;
}

export async function DeleteDeepIntraVMType(id: number) {
  let api = new IntraVMTypeApi();
  setLoader("ADD", "DeleteDeepIntraVMType");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.IntraVMTypeDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_INTRAVMTYPE", payload: rtn });
  setLoader("REMOVE", "DeleteDeepIntraVMType");
  return rtn;
}
