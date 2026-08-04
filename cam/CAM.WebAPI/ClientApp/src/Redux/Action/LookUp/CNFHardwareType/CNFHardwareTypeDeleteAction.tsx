import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CNFHardwareTypeApi } from "../../../../Business/LookUp/CNFHardwareTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteCNFHardwareType(id: number) {
  setLoader("ADD", "DeleteCNFHardwareType");
  let api = new CNFHardwareTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFHardwareTypeDelete(id)
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
  rootStore.dispatch({ type: "DELETE_CNFHARDWARETYPE", payload: rtn });
  setLoader("REMOVE", "DeleteCNFHardwareType");
  return rtn;
}

export async function DeleteDeepCNFHardwareType(id: number) {
  let api = new CNFHardwareTypeApi();
  setLoader("ADD", "DeleteDeepCNFHardwareType");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CNFHardwareTypeDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_CNFHARDWARETYPE", payload: rtn });
  setLoader("REMOVE", "DeleteDeepCNFHardwareType");
  return rtn;
}
