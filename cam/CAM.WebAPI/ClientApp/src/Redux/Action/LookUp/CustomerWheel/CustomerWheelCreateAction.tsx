import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CustomerWheelApi } from "../../../../Business/LookUp/CustomerWheel";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpCreate,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetCustomerWheelCreateResource() {
  setLoader("ADD", "GetCustomerWheelCreateResource");

  let api = new CustomerWheelApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.customerWheelGetCreateResourceCustomerWheel());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({
    type: "GET_CREATE_CUSTOMER_WHEEL",
    payload: rtn,
  });
  setLoader("REMOVE", "GetCustomerWheelCreateResource");
}

export async function CreatCustomerWheel(data: TipologicaGridDto) {
  let api = new CustomerWheelApi();
  setLoader("ADD", "CreatCustomerWheel");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.customerWheelCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_CUSTOMER_WHEEL", payload: rtn });
  setLoader("REMOVE", "CreatCustomerWheel");
  return rtn;
}
