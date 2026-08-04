import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { PlannedActivityApi } from "../../../Business/PlannedActivityBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { EDIT_PLANNED_ACTIVITY, GET_EDIT_PLANNED_ACTIVITY, PlannedActivityDtoUpdate, PlannedActivityEdit } from "../../../Model/PlannedActivity";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetPlannedActivityEditResource(id: number) {
	setLoader("ADD", "GetPlannedActivityEditResource");

	let api = new PlannedActivityApi();
	let createResource = await ApiCallWithErrorHandling<Promise<PlannedActivityDtoUpdate>>(() => api.plannedActivityGetUpdateResourcePlannedActivity(id));
	let rtn = { PlannedActivityDtoEdit: createResource } as PlannedActivityEdit;
	rootStore.dispatch({ type: GET_EDIT_PLANNED_ACTIVITY, payload: rtn });
	setLoader("REMOVE", "GetPlannedActivityEditResource");

	return createResource;
}

export async function EditPlannedActivity(data: PlannedActivityDtoUpdate) {
	setLoader("ADD", "EditPlannedActivity");
	let api = new PlannedActivityApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.plannedActivityPut(data));
	let rtn = { ResultDtoEdit: result } as PlannedActivityEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: EDIT_PLANNED_ACTIVITY, payload: rtn });
	setLoader("REMOVE", "EditPlannedActivity");
	return rtn;
}
