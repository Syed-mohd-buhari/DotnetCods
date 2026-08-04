import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { LcmEngineeringApi } from "../../../Business/LcmEngineeringBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  DELETE_LCM_ENGINEERING,
  RESTORE_LCM_ENGINEERING,
} from "../../../Model/LcmEngineering";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteLcmEngineering(id: number) {
  setLoader("ADD", "deleteLcmEngineering");
  let api = new LcmEngineeringApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.lcmEngineeringDelete(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: DELETE_LCM_ENGINEERING, payload: rtn });

  setLoader("REMOVE", "deleteLcmEngineering");
  return rtn;
}

export async function DeleteDeepLcmEngineering(
  id: number,
  deleteOnlyPlannedActivity: boolean
) {
  let api = new LcmEngineeringApi();
  setLoader("ADD", "DeleteDeepLcmEngineering");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.lcmEngineeringDeleteDeep(id, deleteOnlyPlannedActivity)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: DELETE_LCM_ENGINEERING, payload: rtn });
  setLoader("REMOVE", "DeleteDeepLcmEngineering");
  return rtn;
}

export async function GetRelatedRecordsLcmEngineering(id: number) {
  let api = new LcmEngineeringApi();
  setLoader("ADD", "GetRelatedRecordsLcmEngineering");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.lcmEngineeringGetRelatedRecords(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  if (result?.warning)
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: NotifyType.error,
      })
    );
  setLoader("REMOVE", "GetRelatedRecordsLcmEngineering");
  return rtn;
}

export async function RestoreLcmEngineering(id: number) {
  setLoader("ADD", "RestoreLcmEngineering");
  let api = new LcmEngineeringApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.lcmEngineeringRestore(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: RESTORE_LCM_ENGINEERING, payload: rtn });

  setLoader("REMOVE", "RestoreLcmEngineering");
  return rtn;
}
