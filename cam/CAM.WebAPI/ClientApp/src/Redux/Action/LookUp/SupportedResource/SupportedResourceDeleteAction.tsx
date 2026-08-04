import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SupportedResourceApi } from "../../../../Business/LookUp/SupportedResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteSupportedResource(id: number) {
	setLoader("ADD", "deleteSupportedResource");
	let api = new SupportedResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.supportedResourceDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_SUPPORTED_RESOURCE", payload: rtn });
	setLoader("REMOVE", "deleteSupportedResource");
	return rtn;
}

export async function DeleteDeepSupportedResource(id: number) {
	let api = new SupportedResourceApi();
	setLoader("ADD", "DeleteDeepSupportedResource");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.supportedResourceDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_SUPPORTED_RESOURCE", payload: rtn });
	setLoader("REMOVE", "DeleteDeepSupportedResource");
	return rtn;
}

export async function GetRelatedRecordsSupportedResource(id: number) {
	let api = new SupportedResourceApi();
	setLoader("ADD", "GetRelatedRecordsSupportedResource");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.supportedResourceGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsSupportedResource");
	return rtn;
}
