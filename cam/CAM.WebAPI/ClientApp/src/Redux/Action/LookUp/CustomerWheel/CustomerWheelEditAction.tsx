import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CustomerWheelApi } from "../../../../Business/LookUp/CustomerWheel";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetCustomerWheelEditResource(id: number) {
  setLoader("ADD", "GetCustomerWheelEditResource");

  let api = new CustomerWheelApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.customerWheelGetUpdateResourceCustomerWheel(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_CUSTOMER_WHEEL", payload: rtn });
  setLoader("REMOVE", "GetCustomerWheelEditResource");

  return rtn;
}

export async function EditCustomerWheel(data: TipologicaGridDto) {
  setLoader("ADD", "EditCustomerWheel");
  let api = new CustomerWheelApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.customerWheelPut(data)
  );
  let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_CUSTOMER_WHEEL", payload: rtn });
  setLoader("REMOVE", "EditCustomerWheel");
  return rtn;
}
