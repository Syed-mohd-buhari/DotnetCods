import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VNFClusterNameApi } from "../../../../Business/LookUp/VNFClusterNameBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import {
  VNFClusterNameCreate,
  VNFClusterNameDto,
} from "../../../../Model/LookUp/VNFClusterName";

export async function GetVNFClusterNameCreateResource() {
  setLoader("ADD", "GetVNFClusterNameCreateResource");

  let api = new VNFClusterNameApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<VNFClusterNameDto>
  >(() => api.VNFClusterNameGetCreatepage());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as VNFClusterNameCreate;
  rootStore.dispatch({ type: "GET_CREATE_VNFCLUSTERNAME", payload: rtn });
  setLoader("REMOVE", "GetVNFClusterNameCreateResource");
}

export async function CreatVNFClusterName(data: VNFClusterNameDto) {
  setLoader("ADD", "CreatVNFClusterName");
  let api = new VNFClusterNameApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VNFClusterNameCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as VNFClusterNameCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_VNFCLUSTERNAME", payload: rtn });
  setLoader("REMOVE", "CreatVNFClusterName");
  return rtn;
}
