import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignAspectApi } from "../../../Business/DesignAspectsBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  DELETE_DESIGN_ASPECT,
  RESTORE_DESIGN_ASPECT,
} from "../../../Model/DesignAspects";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteDesignAspect(id: number) {
  setLoader("ADD", "deleteDesignAspect");
  let api = new DesignAspectApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.designAspectDelete(id)
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
  rootStore.dispatch({ type: DELETE_DESIGN_ASPECT, payload: rtn });

  setLoader("REMOVE", "deleteDesignAspect");
  return rtn;
}

export async function DeleteDeepDesignAspect(
  id: number,
  deleteOnlyPlannedActivity: boolean
) {
  let api = new DesignAspectApi();
  setLoader("ADD", "DeleteDeepDesignAspect");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.designAspectDeleteDeep(id, deleteOnlyPlannedActivity)
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
  rootStore.dispatch({ type: DELETE_DESIGN_ASPECT, payload: rtn });
  setLoader("REMOVE", "DeleteDeepDesignAspect");
  return rtn;
}

export async function GetRelatedRecordsDesignAspect(id: number) {
  let api = new DesignAspectApi();
  setLoader("ADD", "GetRelatedRecordsDesignAspect");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.designAspectGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsDesignAspect");
  return rtn;
}

export async function RestoreDesignAspect(id: number) {
  setLoader("ADD", "RestoreDesignAspect");
  let api = new DesignAspectApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.designAspectRestore(id)
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
  rootStore.dispatch({ type: RESTORE_DESIGN_ASPECT, payload: rtn });

  setLoader("REMOVE", "RestoreDesignAspect");
  return rtn;
}
