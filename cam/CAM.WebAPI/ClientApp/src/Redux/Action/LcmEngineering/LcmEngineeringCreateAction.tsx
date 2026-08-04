import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { LcmEngineeringApi } from "../../../Business/LcmEngineeringBusiness";
import { getResourceObject } from "../../../Model/Common";
import { ResultDto } from "../../../Model/CommonModels";
import {
  CREATE_LCM_ENGINEERING,
  GET_CREATE_LCM_ENGINEERING,
  LcmEngineeringCreate,
  LcmEngineeringDtoCreate,
} from "../../../Model/LcmEngineering";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetLcmEngineeringCreateResource({
  isRefillData,
}: {
  isRefillData?: boolean;
} = {}) {
  setLoader("ADD", "GetLcmEngineeringCreateResource");
  // const dispach = useDispatch();
  let api = new LcmEngineeringApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<LcmEngineeringDtoCreate>
  >(() => api.lcmEngineeringGetCreateResourceLcmengineering());
  let rtn = {
    ResultDtoCreate: null,
    LcmEngineeringDtoCreate: createResource,
  } as LcmEngineeringCreate;
  if (!isRefillData || isRefillData === undefined) {
    rootStore.dispatch({ type: GET_CREATE_LCM_ENGINEERING, payload: rtn });
  }
  setLoader("REMOVE", "GetLcmEngineeringCreateResource");
  return rtn;
}

export async function GetLCMResourceKey(obj: getResourceObject) {
  setLoader("ADD", "GetLCMResourceKey");
  let api = new LcmEngineeringApi();
  let getKey = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.getResourceKeyForLCM(obj)
  );
  setLoader("REMOVE", "GetLCMResourceKey");
  return getKey;
}

export async function CreatLcmEngineering(
  data: LcmEngineeringDtoCreate,
  forced?: boolean
) {
  setLoader("ADD", "CreatLcmEngineering");
  let api = new LcmEngineeringApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.lcmEngineeringCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    LcmEngineeringDtoCreate: null,
  } as LcmEngineeringCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_LCM_ENGINEERING, payload: rtn });
  setLoader("REMOVE", "CreatLcmEngineering");
  return rtn;
}
