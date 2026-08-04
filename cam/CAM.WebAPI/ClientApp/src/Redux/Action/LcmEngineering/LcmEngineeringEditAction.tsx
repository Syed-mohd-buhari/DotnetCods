import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { LcmEngineeringApi } from "../../../Business/LcmEngineeringBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  EDIT_LCM_ENGINEERING,
  GET_EDIT_LCM_ENGINEERING,
  LcmEngineeringDtoUpdate,
  LcmEngineeringEdit,
} from "../../../Model/LcmEngineering";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetLcmEngineeringEditResource(id: number) {
  setLoader("ADD", "GetLcmEngineeringEditResource");
  let api = new LcmEngineeringApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<LcmEngineeringDtoUpdate>
  >(() => api.lcmEngineeringGetUpdateResourceLcmengineering(id));
  let rtn = { LcmEngineeringDtoEdit: createResource } as LcmEngineeringEdit;
  rootStore.dispatch({ type: GET_EDIT_LCM_ENGINEERING, payload: rtn });
  setLoader("REMOVE", "GetLcmEngineeringEditResource");
}

export async function EditLcmEngineering(
  data: LcmEngineeringDtoUpdate,
  forced?: boolean
) {
  let api = new LcmEngineeringApi();
  setLoader("ADD", "EditLcmEngineering");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.lcmEngineeringPut(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as LcmEngineeringEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_LCM_ENGINEERING, payload: rtn });
  setLoader("REMOVE", "EditLcmEngineering");
  return rtn;
}
