import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlannedActivityResourceApi } from "../../../../Business/LookUp/PlannedActivityResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deletePlannedActivityResource(id: number) {
	setLoader("ADD", "deletePlannedActivityResource");
	let api = new PlannedActivityResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.plannedActivityResourceDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_PLANNED_ACTIVITY_RESOURCE", payload: rtn });
	setLoader("REMOVE", "deletePlannedActivityResource");
	return rtn;
}

export async function DeleteDeepPlannedActivityResource(id: number) {
	let api = new PlannedActivityResourceApi();
	setLoader("ADD", "DeleteDeepPlannedActivityResource");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.plannedActivityResourceDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_PLANNED_ACTIVITY_RESOURCE", payload: rtn });
	setLoader("REMOVE", "DeleteDeepPlannedActivityResource");
	return rtn;
}

export async function GetRelatedRecordsPlannedActivityResource(id: number) {
	let api = new PlannedActivityResourceApi();
	setLoader("ADD", "GetRelatedRecordsPlannedActivityResource");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.plannedActivityResourceGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsPlannedActivityResource");
	return rtn;
}
