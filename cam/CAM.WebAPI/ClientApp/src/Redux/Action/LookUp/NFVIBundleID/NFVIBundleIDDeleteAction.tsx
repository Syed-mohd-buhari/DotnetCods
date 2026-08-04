import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NFVIBundleIDApi } from "../../../../Business/LookUp/NFVIBundleIDBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteNFVIBundleID(id: number) {
	setLoader("ADD", "deleteNFVIBundleID");
	let api = new NFVIBundleIDApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVIBundleIDDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_NFVI_BUNDLE_ID", payload: rtn });
	setLoader("REMOVE", "deleteNFVIBundleID");
	return rtn;
}

export async function DeleteDeepNFVIBundleID(id: number) {
	let api = new NFVIBundleIDApi();
	setLoader("ADD", "DeleteDeepNFVIBundleID");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVIBundleIDDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_NFVI_BUNDLE_ID", payload: rtn });
	setLoader("REMOVE", "DeleteDeepNFVIBundleID");
	return rtn;
}

export async function GetRelatedRecordsNFVIBundleID(id: number) {
	let api = new NFVIBundleIDApi();
	setLoader("ADD", "GetRelatedRecordsNFVIBundleID");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVIBundleIDGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsNFVIBundleID");
	return rtn;
}
