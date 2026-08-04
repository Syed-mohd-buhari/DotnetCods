import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ServiceBoundaryApi } from "../../../../Business/LookUp/ServiceBoundaryBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";
import { ServiceBoundaryGridDto } from "../../../../Model/LookUp/ServiceBoundary";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetServiceBoundaryEditResource(id: number) {
  setLoader("ADD", "GetServiceBoundaryEditResource");

  let api = new ServiceBoundaryApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<ServiceBoundaryGridDto>
  >(() => api.serviceBoundaryGetUpdateResourceServiceBoundary(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_SUBNETWORK_BOUNDARY", payload: rtn });
  setLoader("REMOVE", "GetServiceBoundaryEditResource");

  return rtn;
}

export async function EditServiceBoundary(data: ServiceBoundaryGridDto) {
  setLoader("ADD", "EditServiceBoundary");
  let api = new ServiceBoundaryApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.serviceBoundaryPut(data)
  );
  let rtn = { ResultDtoEdit: result } as ServiceBoundaryGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_SERVICE_BOUNDARY", payload: rtn });
  setLoader("REMOVE", "EditServiceBoundary");
  return rtn;
}
