import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { InterVMTypeApi } from "../../../../Business/LookUp/InterVMTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteInterVMType(id: number) {
  setLoader("ADD", "DeleteInterVMType");
  let api = new InterVMTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.InterVMTypeDelete(id)
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
  rootStore.dispatch({ type: "DELETE_INTERVMTYPE", payload: rtn });
  setLoader("REMOVE", "DeleteInterVMType");
  return rtn;
}

export async function DeleteDeepInterVMType(id: number) {
  let api = new InterVMTypeApi();
  setLoader("ADD", "DeleteDeepInterVMType");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.InterVMTypeDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_INTERVMTYPE", payload: rtn });
  setLoader("REMOVE", "DeleteDeepInterVMType");
  return rtn;
}
