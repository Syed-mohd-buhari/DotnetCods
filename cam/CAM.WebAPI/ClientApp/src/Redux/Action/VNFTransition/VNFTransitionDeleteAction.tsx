import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { VNFTransitionApi } from "../../../Business/VNFTransitionBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_VNF_TRANSITION, RESTORE_VNF_TRANSITION } from "../../../Model/VNFTransition";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteVNFTransition(id: number) {
	setLoader("ADD", "deleteVNFTransition");
	let api = new VNFTransitionApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.vNFTransitionDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_VNF_TRANSITION, payload: rtn });
	setLoader("REMOVE", "deleteVNFTransition");
	return rtn;
}

export async function DeleteDeepVNFTransition(id: number) {
	let api = new VNFTransitionApi();
	setLoader("ADD", "DeleteDeepVNFTransition");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.vNFTransitionDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_VNF_TRANSITION, payload: rtn });
	setLoader("REMOVE", "DeleteDeepVNFTransition");
	return rtn;
}

export async function GetRelatedRecordsVNFTransition(id: number) {
	let api = new VNFTransitionApi();
	setLoader("ADD", "GetRelatedRecordsVNFTransition");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.vNFTransitionGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsVNFTransition");
	return rtn;
}

export async function RestoreVNFTransition(id: number) {
	setLoader("ADD", "RestoreVNFTransition");
	let api = new VNFTransitionApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.vNFTransitionRestore(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: RESTORE_VNF_TRANSITION, payload: rtn });
	setLoader("REMOVE", "RestoreVNFTransition");
	return rtn;
}
