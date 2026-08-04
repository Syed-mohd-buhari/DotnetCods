import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlanningActivityStatusApi } from "../../../../Business/LookUp/PlanningActivityStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDtoProjectStatusCombinationRule } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetPlanningActivityStatusEditResource(id: number) {
	setLoader("ADD", "GetPlanningActivityStatusEditResource");

	let api = new PlanningActivityStatusApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDtoProjectStatusCombinationRule>>(() => api.planningActivityStatusGetUpdateResourcePlannedActivityStatus(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_PLANNING_ACTIVITY_STATUS", payload: rtn });
	setLoader("REMOVE", "GetPlanningActivityStatusEditResource");

	return rtn;
}

export async function EditPlanningActivityStatus(data: TipologicaGridDtoProjectStatusCombinationRule) {
	setLoader("ADD", "EditPlanningActivityStatus");
	let api = new PlanningActivityStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.planningActivityStatusPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDtoProjectStatusCombinationRule;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_PLANNING_ACTIVITY_STATUS", payload: rtn });
	setLoader("REMOVE", "EditPlanningActivityStatus");
	return rtn;
}
