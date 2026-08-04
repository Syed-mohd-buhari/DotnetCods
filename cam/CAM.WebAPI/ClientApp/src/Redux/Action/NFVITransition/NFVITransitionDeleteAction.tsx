import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NFVITransitionApi } from "../../../Business/NFVITransitionBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_NFVI_TRAMSITION, RESTORE_NFVI_TRAMSITION } from "../../../Model/NFVITransition";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteNFVITransition(id: number) {
	setLoader("ADD", "deleteNFVITransition");
	let api = new NFVITransitionApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVITransitionDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_NFVI_TRAMSITION, payload: rtn });
	setLoader("REMOVE", "deleteNFVITransition");
	return rtn;
}

export async function DeleteDeepNFVITransition(id: number) {
	let api = new NFVITransitionApi();
	setLoader("ADD", "DeleteDeepNFVITransition");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVITransitionDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_NFVI_TRAMSITION, payload: rtn });
	setLoader("REMOVE", "DeleteDeepNFVITransition");
	return rtn;
}

export async function GetRelatedRecordsNFVITransition(id: number) {
	let api = new NFVITransitionApi();
	setLoader("ADD", "GetRelatedRecordsNFVITransition");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVITransitionGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsNFVITransition");
	return rtn;
}

export async function RestoreNFVITransition(id: number) {
	setLoader("ADD", "RestoreNFVITransition");
	let api = new NFVITransitionApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVITransitionRestore(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: RESTORE_NFVI_TRAMSITION, payload: rtn });
	setLoader("REMOVE", "RestoreNFVITransition");
	return rtn;
}
