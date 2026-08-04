import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ReasonCheckboxResourceApi } from "../../../../Business/LookUp/ReasonCheckboxBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteReasonCheckbox(id: number) {
	setLoader("ADD", "deleteReasonCheckbox");
	let api = new ReasonCheckboxResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.reasonCheckboxResourceDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_REASON_CHECKBOX", payload: rtn });
	setLoader("REMOVE", "deleteReasonCheckbox");
	return rtn;
}

export async function DeleteDeepReasonCheckbox(id: number) {
	let api = new ReasonCheckboxResourceApi();
	setLoader("ADD", "DeleteDeepReasonCheckbox");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.reasonCheckboxResourceDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_REASON_CHECKBOX", payload: rtn });
	setLoader("REMOVE", "DeleteDeepReasonCheckbox");
	return rtn;
}

export async function GetRelatedRecordsReasonCheckbox(id: number) {
	let api = new ReasonCheckboxResourceApi();
	setLoader("ADD", "GetRelatedRecordsReasonCheckbox");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.reasonCheckboxResourceGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsReasonCheckbox");
	return rtn;
}
