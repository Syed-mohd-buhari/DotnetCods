import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { PlannedActivityApi } from "../../../Business/PlannedActivityBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  CREATE_PLANNED_ACTIVITY,
  GET_CREATE_PLANNED_ACTIVITY,
  PlannedActivityCreate,
  PlannedActivityDtoCreate,
} from "../../../Model/PlannedActivity";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetPlannedActivityCreateResource(
  dcId?: number,
  DcfId?: number
) {
  setLoader("ADD", "GetPlannedActivityCreateResource");
  let api = new PlannedActivityApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<PlannedActivityDtoCreate>
  >(() => api.plannedActivityGetCreateResourcePlannedActivity(dcId, DcfId));
  let rtn = {
    ResultDtoCreate: null,
    PlannedActivityDtoCreate: createResource,
  } as PlannedActivityCreate;
  rootStore.dispatch({ type: GET_CREATE_PLANNED_ACTIVITY, payload: rtn });
  setLoader("REMOVE", "GetPlannedActivityCreateResource");
  return rtn;
}
export async function GetPlannedActivityCreateResourceRefill({
  dcId,
  isRefillData,
}: {
  dcId?: number;
  isRefillData?: boolean;
} = {}) {
  setLoader("ADD", "GetPlannedActivityCreateResourceRefill");
  let api = new PlannedActivityApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<PlannedActivityDtoCreate>
  >(() => api.plannedActivityGetCreateResourcePlannedActivity(dcId));
  let rtn = {
    ResultDtoCreate: null,
    PlannedActivityDtoCreate: createResource,
  } as PlannedActivityCreate;
  if (!isRefillData || isRefillData === undefined) {
    rootStore.dispatch({ type: GET_CREATE_PLANNED_ACTIVITY, payload: rtn });
  }
  setLoader("REMOVE", "GetPlannedActivityCreateResourceRefill");
  return rtn;
}

export async function CreatPlannedActivity(data: PlannedActivityDtoCreate) {
  setLoader("ADD", "CreatPlannedActivity");
  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.plannedActivityCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    PlannedActivityDtoCreate: null,
  } as PlannedActivityCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_PLANNED_ACTIVITY, payload: rtn });
  setLoader("REMOVE", "CreatPlannedActivity");
  return rtn;
}

export async function GetLocationTypeById(id: number) {
  setLoader("ADD", "GetLocationTypeById");
  let api = new PlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.getLocationTypeById(id)
  );

  setLoader("REMOVE", "GetLocationTypeById");
  return result;
}
