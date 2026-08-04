import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ResponsibilityPhaseApi } from "../../../../Business/LookUp/ResponsibilityPhaseBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function deleteResponsibilityPhase(id: number) {
	setLoader("ADD", "deleteResponsibilityPhase");
	let api = new ResponsibilityPhaseApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.responsibilityPhaseDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_RESPONSABILITY_PHASE", payload: rtn });
	setLoader("REMOVE", "deleteResponsibilityPhase");
	return rtn;
}

export async function DeleteDeepResponsibilityPhase(id: number) {
	let api = new ResponsibilityPhaseApi();
	setLoader("ADD", "DeleteDeepResponsibilityPhase");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.responsibilityPhaseDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_RESPONSABILITY_PHASE", payload: rtn });
	setLoader("REMOVE", "DeleteDeepResponsibilityPhase");
	return rtn;
}

export async function GetRelatedRecordsResponsibilityPhase(id: number) {
	let api = new ResponsibilityPhaseApi();
	setLoader("ADD", "GetRelatedRecordsResponsibilityPhase");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.responsibilityPhaseGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsResponsibilityPhase");
	return rtn;
}
