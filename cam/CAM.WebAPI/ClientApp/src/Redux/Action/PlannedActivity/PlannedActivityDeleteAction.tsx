import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { PlannedActivityApi } from "../../../Business/PlannedActivityBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_PLANNED_ACTIVITY } from "../../../Model/PlannedActivity";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deletePlannedActivity(id: number) {
  setLoader("ADD", "deletePlannedActivity");
  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityDelete(id)
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
  rootStore.dispatch({ type: DELETE_PLANNED_ACTIVITY, payload: rtn });

  setLoader("REMOVE", "deletePlannedActivity");
  return rtn;
}

export async function DeleteDeepPlannedActivity(id: number) {
  let api = new PlannedActivityApi();
  setLoader("ADD", "DeleteDeepPlannedActivity");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityDeleteDeep(id)
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
  rootStore.dispatch({ type: DELETE_PLANNED_ACTIVITY, payload: rtn });
  setLoader("REMOVE", "DeleteDeepPlannedActivity");
  return rtn;
}

export async function GetRelatedRecordsPlannedActivity(id: number) {
  let api = new PlannedActivityApi();
  setLoader("ADD", "GetRelatedRecordsPlannedActivity");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsPlannedActivity");
  return rtn;
}
