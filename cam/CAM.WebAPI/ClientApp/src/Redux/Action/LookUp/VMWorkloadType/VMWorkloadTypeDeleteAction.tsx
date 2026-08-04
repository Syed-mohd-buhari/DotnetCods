import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VMWorkloadTypeApi } from "../../../../Business/LookUp/VMWorkloadTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteVMWorkloadType(id: number) {
  setLoader("ADD", "DeleteVMWorkloadType");
  let api = new VMWorkloadTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VMWorkloadTypeDelete(id)
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
  rootStore.dispatch({ type: "DELETE_VMWORKLOADTYPE", payload: rtn });
  setLoader("REMOVE", "DeleteVMWorkloadType");
  return rtn;
}

export async function DeleteDeepVMWorkloadType(id: number) {
  let api = new VMWorkloadTypeApi();
  setLoader("ADD", "DeleteDeepVMWorkloadType");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VMWorkloadTypeDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_VMWORKLOADTYPE", payload: rtn });
  setLoader("REMOVE", "DeleteDeepVMWOrkloadType");
  return rtn;
}
