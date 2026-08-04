import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ServiceMasterApi } from "../../../../Business/LookUp/ServiceMasterBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteServiceMaster(id: number) {
  setLoader("ADD", "DeleteServiceMaster");
  let api = new ServiceMasterApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.ServiceMasterDelete(id)
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
  rootStore.dispatch({ type: "DELETE_SERVICEMASTER", payload: rtn });
  setLoader("REMOVE", "DeleteServiceMaster");
  return rtn;
}

export async function DeleteDeepServiceMaster(id: number) {
  let api = new ServiceMasterApi();
  setLoader("ADD", "DeleteDeepServiceMaster");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.ServiceMasterDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_SERVICEMASTER", payload: rtn });
  setLoader("REMOVE", "DeleteDeepServiceMaster");
  return rtn;
}
