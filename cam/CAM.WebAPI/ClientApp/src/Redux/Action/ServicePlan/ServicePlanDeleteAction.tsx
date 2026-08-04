import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ServicePlanApi } from "../../../Business/ServicePlanBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteServicePlan(id: number) {
  setLoader("ADD", "DeleteServicePlan");
  let api = new ServicePlanApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.ServicePlanDelete(id)
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
  rootStore.dispatch({ type: "DELETE_SERVICE_PLAN", payload: rtn });
  setLoader("REMOVE", "DeleteServicePlan");
  return rtn;
}

export async function DeleteDeepServicePlan(id: number) {
  let api = new ServicePlanApi();
  setLoader("ADD", "DeleteDeepServicePlan");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.ServicePlanDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_SERVICE_PLAN", payload: rtn });
  setLoader("REMOVE", "DeleteDeepServicePlan");
  return rtn;
}
