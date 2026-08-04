import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SubNetworkBoundaryApi } from "../../../../Business/LookUp/SubnetworkBoundryBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";
import { SubNetworkBoundaryGridDto } from "../../../../Model/LookUp/SubnetworkBoundry";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSubNetworkBoundaryEditResource(id: number) {
  setLoader("ADD", "GetSubNetworkBoundaryEditResource");

  let api = new SubNetworkBoundaryApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<SubNetworkBoundaryGridDto>
  >(() => api.subNetworkBoundaryGetUpdateResourceSubNetworkBoundary(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_SUBNETWORK_BOUNDARY", payload: rtn });
  setLoader("REMOVE", "GetSubNetworkBoundaryEditResource");

  return rtn;
}

export async function EditSubNetworkBoundary(data: SubNetworkBoundaryGridDto) {
  setLoader("ADD", "EditSubNetworkBoundary");
  let api = new SubNetworkBoundaryApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.subNetworkBoundaryPut(data)
  );
  let rtn = { ResultDtoEdit: result } as SubNetworkBoundaryGridDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_SUBNETWORK_BOUNDARY", payload: rtn });
  setLoader("REMOVE", "EditSubNetworkBoundary");
  return rtn;
}
