import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ServiceBoundaryApi } from "../../../../Business/LookUp/ServiceBoundaryBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import {
  LookUpCreateServiceBoundary,
  ServiceBoundaryGridDto,
} from "../../../../Model/LookUp/ServiceBoundary";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetServiceBoundaryCreateResource() {
  setLoader("ADD", "GetServiceBoundaryCreateResource");

  let api = new ServiceBoundaryApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.serviceBoundaryGetCreateResourceServiceBoundary());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreateServiceBoundary;
  rootStore.dispatch({ type: "GET_CREATE_SERVICE_BOUNDARY", payload: rtn });
  setLoader("REMOVE", "GetServiceBoundaryCreateResource");
}

export async function CreatServiceBoundary(data: ServiceBoundaryGridDto) {
  setLoader("ADD", "CreatServiceBoundary");
  let api = new ServiceBoundaryApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.serviceBoundaryCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as LookUpCreateServiceBoundary;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_SERVICE_BOUNDARY", payload: rtn });
  setLoader("REMOVE", "CreatServiceBoundary");
  return rtn;
}
