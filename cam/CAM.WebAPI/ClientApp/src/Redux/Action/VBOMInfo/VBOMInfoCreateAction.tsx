import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import {
  VBOMInfoApiFetchParamCreator,
  VBOMInfoApi,
} from "../../../Business/VBOMInfoBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  GET_CREATE_VBOM_INFO,
  CREATE_VBOM_INFO,
  VBOMInfoCreate,
  VBOMInfoDtoCreate,
  CREATE_VBOM_CLUSTER_INFO,
  VBOMClusterInfoDtoCreate,
  VBOMClusterInfoCreate,
  GET_CREATE_VBOM_CLUSTER_INFO,
} from "../../../Model/VBOMInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetVBOMInfoCreateResource() {
  setLoader("ADD", "GetVBOMInfoCreateResource");

  let api = new VBOMInfoApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<VBOMInfoDtoCreate>
  >(() => api.VBOMInfoGetCreateResourceVBOMInfo());
  let rtn = {
    ResultDtoCreate: null,
    VBOMInfoDtoCreate: createResource,
  } as VBOMInfoCreate;
  let rtn1 = {
    ResultDtoCreate: null,
    VBOMClusterInfoDtoCreate: createResource,
  } as VBOMClusterInfoCreate;
  rootStore.dispatch({ type: GET_CREATE_VBOM_INFO, payload: rtn });
  rootStore.dispatch({ type: GET_CREATE_VBOM_CLUSTER_INFO, payload: rtn1 });
  setLoader("REMOVE", "GetVBOMInfoCreateResource");

  return rtn1.VBOMClusterInfoDtoCreate;
}

export async function CreateResourceVBOMInfoRefillData() {
  setLoader("ADD", "GetVBOMInfoCreateResource");

  let api = new VBOMInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<VBOMInfoDtoCreate>>(() =>
    api.VBOMInfoGetCreateResourceVBOMInfo()
  );
  setLoader("REMOVE", "GetVBOMInfoCreateResource");

  return result;
}

export async function CreateVBOMInfo(
  data: VBOMInfoDtoCreate,
  forced?: boolean
) {
  setLoader("ADD", "CreateVBOMInfo");
  let api = new VBOMInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VBOMInfoCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    VBOMInfoDtoCreate: null,
  } as VBOMInfoCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_VBOM_INFO, payload: rtn });
  setLoader("REMOVE", "CreateVBOMInfo");
  return rtn;
}

export async function CreateVBOMClusterInfo(
  data: VBOMClusterInfoDtoCreate,
  forced?: boolean
) {
  setLoader("ADD", "CreateVBOMClusterInfo");
  let api = new VBOMInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.VBOMInfoCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    VBOMClusterInfoDtoCreate: null,
  } as VBOMClusterInfoCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  console.log("Resturn", result);
  rootStore.dispatch({ type: CREATE_VBOM_CLUSTER_INFO, payload: rtn });
  setLoader("REMOVE", "CreateVBOMClusterInfo");
  return rtn;
}
