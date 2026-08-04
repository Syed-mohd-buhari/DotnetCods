import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlanningActivityStatusApi } from "../../../../Business/LookUp/PlanningActivityStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDtoProjectStatusCombinationRule } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetPlanningActivityStatusCreateResource() {
	setLoader("ADD", "GetPlanningActivityStatusCreateResource");

	let api = new PlanningActivityStatusApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDtoProjectStatusCombinationRule>>(() => api.planningActivityStatusGetCreateResourcePlannedActivityStatus());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_PLANNING_ACTIVITY_STATUS", payload: rtn });
	setLoader("REMOVE", "GetPlanningActivityStatusCreateResource");
}

export async function CreatPlanningActivityStatus(data: TipologicaGridDtoProjectStatusCombinationRule) {
	setLoader("ADD", "CreatPlanningActivityStatus");
	let api = new PlanningActivityStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.planningActivityStatusCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_PLANNING_ACTIVITY_STATUS", payload: rtn });
	setLoader("REMOVE", "CreatPlanningActivityStatus");
	return rtn;
}
