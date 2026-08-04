import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NetworkFunctionApi } from "../../../../Business/LookUp/NetworkFunctionBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpCreate,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetNetworkFunctionCreateResource() {
  setLoader("ADD", "GetNetworkFunctionCreateResource");

  let api = new NetworkFunctionApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.networkFunctionGetCreateResourceNetworkFunction());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({ type: "GET_CREATE_NETWORK_FUNCTION", payload: rtn });
  setLoader("REMOVE", "GetNetworkFunctionCreateResource");
}

export async function CreatNetworkFunction(data: TipologicaGridDto) {
  setLoader("ADD", "CreatNetworkFunction");
  let api = new NetworkFunctionApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkFunctionCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_NETWORK_FUNCTION", payload: rtn });
  setLoader("REMOVE", "CreatNetworkFunction");
  return rtn;
}
