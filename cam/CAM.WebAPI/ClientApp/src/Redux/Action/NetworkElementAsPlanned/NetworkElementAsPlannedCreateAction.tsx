import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NetworkElementAsPlannedApi } from "../../../Business/NetworkElementAsPlannedBusiness";
import { getLcmIdObject, getResourceObject } from "../../../Model/Common";
import { ResultDto } from "../../../Model/CommonModels";
import {
  CREATE_NETWORK_ELEMENT_AS_PLANNED,
  GET_CREATE_NETWORK_ELEMENT_AS_PLANNED,
  NetworkElementAsPlannedCreate,
  NetworkElementAsPlannedDtoCreate,
} from "../../../Model/NetworkElementAsPlanned";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetNetworkElementAsPlannedCreateResource({
  isRefillData,
}: {
  isRefillData?: boolean;
} = {}) {
  // const dispach = useDispatch();
  setLoader("ADD", "GetNetworkElementAsPlannedCreateResource");
  let api = new NetworkElementAsPlannedApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<NetworkElementAsPlannedDtoCreate>
  >(() => api.networkElementAsPlannedGetCreateResourceLcmengineering());
  let rtn = {
    ResultDtoCreate: null,
    NetworkElementAsPlannedDtoCreate: createResource,
  } as NetworkElementAsPlannedCreate;
  if (!isRefillData || isRefillData === undefined) {
    rootStore.dispatch({
      type: GET_CREATE_NETWORK_ELEMENT_AS_PLANNED,
      payload: rtn,
    });
  }
  setLoader("REMOVE", "GetNetworkElementAsPlannedCreateResource");
  return rtn.NetworkElementAsPlannedDtoCreate as NetworkElementAsPlannedDtoCreate;
}

export async function CreatNetworkElementAsPlanned(
  data: NetworkElementAsPlannedDtoCreate,
  forced?: boolean
) {
  setLoader("ADD", "CreatNetworkElementAsPlanned");
  let api = new NetworkElementAsPlannedApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.networkElementAsPlannedCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    NetworkElementAsPlannedDtoCreate: null,
  } as NetworkElementAsPlannedCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_NETWORK_ELEMENT_AS_PLANNED, payload: rtn });
  setLoader("REMOVE", "CreatNetworkElementAsPlanned");
  return rtn;
}
export async function GetAssetResourceKey(obj: getResourceObject) {
  setLoader("ADD", "GetAssetResourceKey");
  let api = new NetworkElementAsPlannedApi();
  let getKey = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.getResourceKeyForAsset(obj)
  );
  setLoader("REMOVE", "GetAssestResourceKey");
  return getKey;
}

export async function GetLCMID(obj: getLcmIdObject) {
  setLoader("ADD", "GetLCMID");
  let api = new NetworkElementAsPlannedApi();
  let getKey = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.GetLCMID(obj)
  );
  setLoader("REMOVE", "GetLCMID");
  return getKey;
}
