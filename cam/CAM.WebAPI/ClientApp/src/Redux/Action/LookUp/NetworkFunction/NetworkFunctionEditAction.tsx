import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NetworkFunctionApi } from "../../../../Business/LookUp/NetworkFunctionBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetNetworkFunctionEditResource(id: number) {
  setLoader("ADD", "GetNetworkFunctionEditResource");

  let api = new NetworkFunctionApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.networkFunctionGetUpdateResourceNetworkFunction(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_NETWORK_FUNCTION", payload: rtn });
  setLoader("REMOVE", "GetNetworkFunctionEditResource");

  return rtn;
}

export async function EditNetworkFunction(data: TipologicaGridDto) {
  setLoader("ADD", "EditNetworkFunction");
  let api = new NetworkFunctionApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkFunctionPut(data)
  );
  let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_NETWORK_FUNCTION", payload: rtn });
  setLoader("REMOVE", "EditNetworkFunction");
  return rtn;
}
