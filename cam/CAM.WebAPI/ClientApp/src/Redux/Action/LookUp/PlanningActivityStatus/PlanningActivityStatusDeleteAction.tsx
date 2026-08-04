import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlanningActivityStatusApi } from "../../../../Business/LookUp/PlanningActivityStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function deletePlanningActivityStatus(id: number) {
	setLoader("ADD", "deletePlanningActivityStatus");
	let api = new PlanningActivityStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.planningActivityStatusDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_PLANNING_ACTIVITY_STATUS", payload: rtn });
	setLoader("REMOVE", "deletePlanningActivityStatus");
	return rtn;
}

export async function DeleteDeepPlanningActivityStatus(id: number) {
	let api = new PlanningActivityStatusApi();
	setLoader("ADD", "DeleteDeepPlanningActivityStatus");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.planningActivityStatusDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_PLANNING_ACTIVITY_STATUS", payload: rtn });
	setLoader("REMOVE", "DeleteDeepPlanningActivityStatus");
	return rtn;
}

export async function GetRelatedRecordsPlanningActivityStatus(id: number) {
	let api = new PlanningActivityStatusApi();
	setLoader("ADD", "GetRelatedRecordsPlanningActivityStatus");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.planningActivityStatusGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsPlanningActivityStatus");
	return rtn;
}
