import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ServiceMasterApi } from "../../../../Business/LookUp/ServiceMasterBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import {
  ServiceMasterCreate,
  ServiceMasterDto,
} from "../../../../Model/LookUp/ServiceMaster";

export async function GetServiceMasterCreateResource() {
  setLoader("ADD", "GetServiceMasterCreateResource");

  let api = new ServiceMasterApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<ServiceMasterDto>
  >(() => api.ServiceMasterGetCreatepage());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as ServiceMasterCreate;
  rootStore.dispatch({ type: "GET_CREATE_SERVICEMASTER", payload: rtn });
  setLoader("REMOVE", "GeServiceMasterCreateResource");
}

export async function CreatServiceMaster(data: ServiceMasterDto) {
  setLoader("ADD", "CreatServiceMaster");
  let api = new ServiceMasterApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.ServiceMasterCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as ServiceMasterCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_SERVICEMASTER", payload: rtn });
  setLoader("REMOVE", "CreatServiceMaster");
  return rtn;
}
