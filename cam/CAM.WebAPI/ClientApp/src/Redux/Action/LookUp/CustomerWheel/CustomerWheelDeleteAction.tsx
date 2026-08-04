import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CustomerWheelApi } from "../../../../Business/LookUp/CustomerWheel";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function deleteCustomerWheel(id: number) {
  setLoader("ADD", "deleteCustomerWheel");
  let api = new CustomerWheelApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.customerWheelDelete(id)
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
  rootStore.dispatch({ type: "DELETE_CUSTOMER_WHEEL", payload: rtn });
  setLoader("REMOVE", "deleteCustomerWheel");
  return rtn;
}

export async function DeleteDeepCustomerWheel(id: number) {
  let api = new CustomerWheelApi();
  setLoader("ADD", "DeleteDeepCustomerWheel");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.customerWheelDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_CUSTOMER_WHEEL", payload: rtn });
  setLoader("REMOVE", "DeleteDeepCustomerWheel");
  return rtn;
}

export async function GetRelatedRecordsCustomerWheel(id: number) {
  let api = new CustomerWheelApi();
  setLoader("ADD", "GetRelatedRecordsCustomerWheel");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.customerWheelGetRelatedRecords(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  if (result?.warning)
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: NotifyType.error,
      })
    );
  setLoader("REMOVE", "GetRelatedRecordsCustomerWheel");
  return rtn;
}
