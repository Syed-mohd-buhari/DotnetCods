import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SharingTypeApi } from "../../../../Business/LookUp/SharingTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteSharingType(id: number) {
	setLoader("ADD", "deleteSharingType");
	let api = new SharingTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.sharingTypeDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_SHARING_TYPE", payload: rtn });
	setLoader("REMOVE", "deleteSharingType");
	return rtn;
}

export async function DeleteDeepSharingType(id: number) {
	let api = new SharingTypeApi();
	setLoader("ADD", "DeleteDeepSharingType");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.sharingTypeDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_SHARING_TYPE", payload: rtn });
	setLoader("REMOVE", "DeleteDeepSharingType");
	return rtn;
}

export async function GetRelatedRecordsSharingType(id: number) {
	let api = new SharingTypeApi();
	setLoader("ADD", "GetRelatedRecordsSharingType");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.sharingTypeGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsSharingType");
	return rtn;
}
