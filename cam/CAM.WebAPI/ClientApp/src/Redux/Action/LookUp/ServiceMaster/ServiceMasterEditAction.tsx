import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ServiceMasterApi } from "../../../../Business/LookUp/ServiceMasterBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  ServiceMasterDto,
  ServiceMasterEdit,
} from "../../../../Model/LookUp/ServiceMaster";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetServiceMasterEditResource(id: number) {
  setLoader("ADD", "GetServiceMasterEditResource");

  let api = new ServiceMasterApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<ServiceMasterDto>
  >(() => api.ServiceMasterGetUpdatedPage(id));
  let rtn = { LookUpDtoEdit: createResource } as ServiceMasterEdit;
  rootStore.dispatch({ type: "GET_EDIT_SERVICEMASTER", payload: rtn });
  setLoader("REMOVE", "GetServiceMasterEditResource");

  return rtn;
}

export async function EditServiceMaster(data: ServiceMasterDto) {
  setLoader("ADD", "EditServiceMaster");
  let api = new ServiceMasterApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.ServiceMasterUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as ServiceMasterEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_SERVICEMASTER", payload: rtn });
  setLoader("REMOVE", "EditServiceMaster");
  return rtn;
}
