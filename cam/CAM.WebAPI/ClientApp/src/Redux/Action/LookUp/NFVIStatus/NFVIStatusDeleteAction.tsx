import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NFVIStatusApi } from "../../../../Business/LookUp/NFVIStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteNFVIStatus(id: number) {
	setLoader("ADD", "deleteNFVIStatus");
	let api = new NFVIStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVIStatusDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_NFVI_STATUS", payload: rtn });
	setLoader("REMOVE", "deleteNFVIStatus");
	return rtn;
}

export async function DeleteDeepNFVIStatus(id: number) {
	let api = new NFVIStatusApi();
	setLoader("ADD", "DeleteDeepNFVIStatus");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVIStatusDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_NFVI_STATUS", payload: rtn });
	setLoader("REMOVE", "DeleteDeepNFVIStatus");
	return rtn;
}

export async function GetRelatedRecordsNFVIStatus(id: number) {
	let api = new NFVIStatusApi();
	setLoader("ADD", "GetRelatedRecordsNFVIStatus");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVIStatusGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsNFVIStatus");
	return rtn;
}
