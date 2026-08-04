import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ActivityStatusApi } from "../../../../Business/LookUp/ActivityStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteActivityStatus(id: number) {
	setLoader("ADD", "deleteActivityStatus");
	let api = new ActivityStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.activityStatusDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_ACTIVITY_STATUS", payload: rtn });
	setLoader("REMOVE", "deleteActivityStatus");

	return rtn;
}

export async function DeleteDeepActivityStatus(id: number) {
	let api = new ActivityStatusApi();
	setLoader("ADD", "DeleteDeepActivityStatus");

	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.activityStatusDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_ACTIVITY_STATUS", payload: rtn });
	setLoader("REMOVE", "DeleteDeepActivityStatus");

	return rtn;
}

export async function GetRelatedRecordsActivityStatus(id: number) {
	let api = new ActivityStatusApi();
	setLoader("ADD", "GetRelatedRecordsActivityStatus");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.activityStatusGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsActivityStatus");
	return rtn;
}
