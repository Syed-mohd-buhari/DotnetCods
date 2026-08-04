import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlatformApi } from "../../../../Business/LookUp/PlatformBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deletePlatform(id: number) {
	setLoader("ADD", "deletePlatform");
	let api = new PlatformApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.platformDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_PLATFORM", payload: rtn });
	setLoader("REMOVE", "deletePlatform");
	return rtn;
}

export async function DeleteDeepPlatform(id: number) {
	let api = new PlatformApi();
	setLoader("ADD", "DeleteDeepPlatform");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.platformDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_PLATFORM", payload: rtn });
	setLoader("REMOVE", "DeleteDeepPlatform");
	return rtn;
}

export async function GetRelatedRecordsPlatform(id: number) {
	let api = new PlatformApi();
	setLoader("ADD", "GetRelatedRecordsPlatform");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.platformGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsPlatform");
	return rtn;
}
