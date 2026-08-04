import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import {
  CBOMApiFetchParamCreator,
  CBOMApi,
} from "../../../Business/CBOMBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  GET_CREATE_CBOM,
  CREATE_CBOM,
  CBOMCreate,
  CBOMDtoCreate,
} from "../../../Model/CBOM";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetCBOMCreateResource() {
  setLoader("ADD", "GetCBOMCreateResource");

  let api = new CBOMApi();
  let createResource = await ApiCallWithErrorHandling<Promise<CBOMDtoCreate>>(
    () => api.CBOMGetCreateResourceCBOM()
  );
  let rtn = {
    ResultDtoCreate: null,
    CBOMDtoCreate: createResource,
  } as CBOMCreate;
  rootStore.dispatch({ type: GET_CREATE_CBOM, payload: rtn });
  setLoader("REMOVE", "GetCBOMCreateResource");

  return rtn.CBOMDtoCreate;
}

export async function CreateResourceCBOMRefillData() {
  setLoader("ADD", "GetCBOMCreateResource");

  let api = new CBOMApi();
  let result = await ApiCallWithErrorHandling<Promise<CBOMDtoCreate>>(() =>
    api.CBOMGetCreateResourceCBOM()
  );
  setLoader("REMOVE", "GetCBOMCreateResource");

  return result;
}

export async function CreateCBOM(data: CBOMDtoCreate, forced?: boolean) {
  setLoader("ADD", "CreateCBOM");
  let api = new CBOMApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.CBOMCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    CBOMDtoCreate: null,
  } as CBOMCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_CBOM, payload: rtn });
  setLoader("REMOVE", "CreateCBOM");
  return rtn;
}
