import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ActivityDetailsApi } from "../../../../Business/LookUp/ActivityDetailsBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteActivityDetails(id: number) {
	setLoader("ADD", "deleteActivityDetails");
	let api = new ActivityDetailsApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.activityDetailsDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_ACTIVITY_DETAILS", payload: rtn });
	setLoader("REMOVE", "deleteActivityDetails");
	return rtn;
}

export async function DeleteDeepActivityDetails(id: number) {
	let api = new ActivityDetailsApi();
	setLoader("ADD", "DeleteDeepActivityDetails");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.activityDetailsDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_ACTIVITY_DETAILS", payload: rtn });
	setLoader("REMOVE", "DeleteDeepActivityDetails");
	return rtn;
}

export async function GetRelatedRecordsActivityDetails(id: number) {
	let api = new ActivityDetailsApi();
	setLoader("ADD", "GetRelatedRecordsActivityDetails");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.activityDetailsGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsActivityDetails");
	return rtn;
}
