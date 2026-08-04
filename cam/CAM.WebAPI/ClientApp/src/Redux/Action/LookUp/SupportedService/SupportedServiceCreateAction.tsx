import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SupportedServiceApi } from "../../../../Business/LookUp/SupportedServiceBusiness";
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

export async function GetSupportedServiceCreateResource() {
  setLoader("ADD", "GetSupportedServiceCreateResource");

  let api = new SupportedServiceApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.supportedServiceGetCreateResourceSupportedService());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({ type: "GET_CREATE_SUPPORTED_SERVICE", payload: rtn });
  setLoader("REMOVE", "GetSupportedServiceCreateResource");
}

export async function CreatSupportedService(data: TipologicaGridDto) {
  setLoader("ADD", "CreatSupportedService");
  let api = new SupportedServiceApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.supportedServiceCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_SUPPORTED_SERVICE", payload: rtn });
  setLoader("REMOVE", "CreatSupportedService");
  return rtn;
}
