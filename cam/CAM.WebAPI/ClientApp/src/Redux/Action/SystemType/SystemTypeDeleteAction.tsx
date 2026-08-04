import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { SystemTypeApi } from "../../../Business/SystemTypeBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_SYSTEM_TYPE, RESTORE_SYSTEM_TYPE } from "../../../Model/SystemTypeModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteSystemType(id: number) {
	setLoader("ADD", "deleteSystemType");
	let api = new SystemTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.systemTypeDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_SYSTEM_TYPE, payload: rtn });
	setLoader("REMOVE", "deleteSystemType");
	return rtn;
}
export async function DeleteDeepSystemType(id: number) {
	let api = new SystemTypeApi();
	setLoader("ADD", "DeleteDeepSystemType");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.systemTypeDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_SYSTEM_TYPE, payload: rtn });
	setLoader("REMOVE", "DeleteDeepSystemType");
	return rtn;
}

export async function RestoreSystemType(id: number) {
	setLoader("ADD", "RestoreSystemType");
	let api = new SystemTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.systemTypeRestore(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: RESTORE_SYSTEM_TYPE, payload: rtn });
	setLoader("REMOVE", "RestoreSystemType");
	return rtn;
}

export async function GetRelatedRecordsSystemType(id: number) {
	let api = new SystemTypeApi();
	setLoader("ADD", "GetRelatedRecordsSystemType");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.systemTypeGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsSystemType");
	return rtn;
}
