import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SupportedServiceApi } from "../../../../Business/LookUp/SupportedServiceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSupportedServiceEditResource(id: number) {
  setLoader("ADD", "GetSupportedServiceEditResource");

  let api = new SupportedServiceApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.supportedServiceGetUpdateResourceSupportedService(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_SUPPORTED_SERVICE", payload: rtn });
  setLoader("REMOVE", "GetSupportedServiceEditResource");

  return rtn;
}

export async function EditSupportedService(data: TipologicaGridDto) {
  setLoader("ADD", "EditSupportedService");
  let api = new SupportedServiceApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.supportedServicePut(data)
  );

  let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_SUPPORTED_SERVICE", payload: rtn });
  setLoader("REMOVE", "EditSupportedService");
  return rtn;
}
