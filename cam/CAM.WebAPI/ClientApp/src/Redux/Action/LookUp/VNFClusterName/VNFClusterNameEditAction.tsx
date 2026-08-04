import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VNFClusterNameApi } from "../../../../Business/LookUp/VNFClusterNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  VNFClusterNameDto,
  VNFClusterNameDtoGrid,
  VNFClusterNameEdit,
} from "../../../../Model/LookUp/VNFClusterName";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetVNFClusterNameEditResource(id: number) {
  setLoader("ADD", "GetVNFClusterNameEditResource");

  let api = new VNFClusterNameApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<VNFClusterNameDto>
  >(() => api.VNFClusterNameGetUpdatedPage(id));
  let rtn = { LookUpDtoEdit: createResource } as VNFClusterNameEdit;
  rootStore.dispatch({ type: "GET_EDIT_VNFCLUSTERNAME", payload: rtn });
  setLoader("REMOVE", "GetVNFClusterNameEditResource");

  return rtn;
}

export async function EditVNFClusterName(data: VNFClusterNameDto) {
  setLoader("ADD", "EditVNFClusterName");
  let api = new VNFClusterNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VNFClusterNameUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as VNFClusterNameEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_VNFCLUSTERNAME", payload: rtn });
  setLoader("REMOVE", "EditVNFClusterName");
  return rtn;
}
