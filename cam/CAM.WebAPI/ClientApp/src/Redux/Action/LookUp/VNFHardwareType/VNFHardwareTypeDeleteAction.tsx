import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VNFHardwareTypeApi } from "../../../../Business/LookUp/VNFHardwareTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteVNFHardwareType(id: number) {
  setLoader("ADD", "DeleteVNFHardwareType");
  let api = new VNFHardwareTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VNFHardwareTypeDelete(id)
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
  rootStore.dispatch({ type: "DELETE_VNFHARDWARETYPE", payload: rtn });
  setLoader("REMOVE", "DeleteVNFHardwareType");
  return rtn;
}

export async function DeleteDeepVNFHardwareType(id: number) {
  let api = new VNFHardwareTypeApi();
  setLoader("ADD", "DeleteDeepVNFHardwareType");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VNFHardwareTypeDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_VNFHARDWARETYPE", payload: rtn });
  setLoader("REMOVE", "DeleteDeepVNFHardwareType");
  return rtn;
}
