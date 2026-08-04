import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import {
  NFVISwCompatibleApiFetchParamCreator,
  NFVISwCompatibleApi,
} from "../../../Business/NFVISoftwareCompatibleBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  GET_CREATE_NFVI_SW_COMPATIBLE,
  CREATE_NFVI_SW_COMPATIBLE,
  NFVISwCompatibleDtoCreate,
  NFVISwCompatibleCreate,
} from "../../../Model/NFVISoftwareCompatible";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetNFVISwCompatibleCreateResource() {
  setLoader("ADD", "GetNFVISwCompatibleCreateResource");

  let api = new NFVISwCompatibleApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<NFVISwCompatibleDtoCreate>
  >(() => api.nfviSwCompatibleGetCreateResourceNFVISwCompatible());
  let rtn = {
    ResultDtoCreate: null,
    NFVISwCompatibleDtoCreate: createResource,
  } as NFVISwCompatibleCreate;
  rootStore.dispatch({ type: GET_CREATE_NFVI_SW_COMPATIBLE, payload: rtn });
  setLoader("REMOVE", "GetNFVISwCompatibleCreateResource");

  return rtn.NFVISwCompatibleDtoCreate;
}

export async function CreatNFVISwCompatible(
  data: NFVISwCompatibleDtoCreate,
  forced?: boolean
) {
  setLoader("ADD", "CreatNFVISwCompatible");
  let api = new NFVISwCompatibleApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.NFVISwCompatibleCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    NFVISwCompatibleDtoCreate: null,
  } as NFVISwCompatibleCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_NFVI_SW_COMPATIBLE, payload: rtn });
  setLoader("REMOVE", "CreatNFVISwCompatible");
  return rtn;
}
