import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlanningRiskApi } from "../../../../Business/LookUp/PlanningRiskBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deletePlanningRisk(id: number) {
	setLoader("ADD", "deletePlanningRisk");
	let api = new PlanningRiskApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.planningRiskDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_PLANNING_RISK", payload: rtn });
	setLoader("REMOVE", "deletePlanningRisk");
	return rtn;
}

export async function DeleteDeepPlanningRisk(id: number) {
	let api = new PlanningRiskApi();
	setLoader("ADD", "DeleteDeepPlanningRisk");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.planningRiskDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_PLANNING_RISK", payload: rtn });
	setLoader("REMOVE", "DeleteDeepPlanningRisk");
	return rtn;
}

export async function GetRelatedRecordsPlanningRisk(id: number) {
	let api = new PlanningRiskApi();
	setLoader("ADD", "GetRelatedRecordsPlanningRisk");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.planningRiskGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsPlanningRisk");
	return rtn;
}
