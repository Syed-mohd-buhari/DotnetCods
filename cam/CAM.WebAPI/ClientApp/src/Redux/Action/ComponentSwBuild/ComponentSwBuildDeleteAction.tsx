import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { ComponentSwBuildApi } from "../../../Business/ComponentSwBuildsBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  DELETE_COMPONENT_SW_BUILD,
  RESTORE_COMPONENT_SW_BUILD,
} from "../../../Model/ComponentSwBuild";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function deleteComponentSwBuild(id: number) {
  setLoader("ADD", "deleteComponentSwBuild");
  let api = new ComponentSwBuildApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.componentSwBuildDelete(id)
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
  rootStore.dispatch({ type: DELETE_COMPONENT_SW_BUILD, payload: rtn });
  setLoader("REMOVE", "deleteComponentSwBuild");
  return rtn;
}
export async function DeleteDeepComponentSwBuild(id: number) {
  let api = new ComponentSwBuildApi();
  setLoader("ADD", "DeleteDeepComponentSwBuild");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.componentSwBuildDeleteDeep(id)
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
  rootStore.dispatch({ type: DELETE_COMPONENT_SW_BUILD, payload: rtn });
  setLoader("REMOVE", "DeleteDeepComponentSwBuild");
  return rtn;
}

export async function RestoreComponentSwBuild(id: number) {
  setLoader("ADD", "RestoreComponentSwBuild");
  let api = new ComponentSwBuildApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.componentSwBuildRestore(id)
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
  rootStore.dispatch({ type: RESTORE_COMPONENT_SW_BUILD, payload: rtn });
  setLoader("REMOVE", "RestoreComponentSwBuild");
  return rtn;
}

export async function GetRelatedRecordsComponentSwBuild(id: number) {
  let api = new ComponentSwBuildApi();
  setLoader("ADD", "GetRelatedRecordsComponentSwBuild");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.componentSwBuildGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsComponentSwBuild");
  return rtn;
}
