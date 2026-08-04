import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { EnvironmentApi } from "../../../../Business/LookUp/EnvironmentBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteEnvironment(id: number) {
	setLoader("ADD", "deleteEnvironment");
	let api = new EnvironmentApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.environmentDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_ENVIRONMENT", payload: rtn });
	setLoader("REMOVE", "deleteEnvironment");
	return rtn;
}

export async function DeleteDeepEnvironment(id: number) {
	let api = new EnvironmentApi();
	setLoader("ADD", "DeleteDeepEnvironment");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.environmentDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_ENVIRONMENT", payload: rtn });
	setLoader("REMOVE", "DeleteDeepEnvironment");
	return rtn;
}

export async function GetRelatedRecordsEnvironment(id: number) {
	let api = new EnvironmentApi();
	setLoader("ADD", "GetRelatedRecordsEnvironment");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.environmentGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsEnvironment");
	return rtn;
}
