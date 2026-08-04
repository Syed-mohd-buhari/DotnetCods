import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NetworkConstructApi } from "../../../../Business/LookUp/NetworkConstructBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteNetworkConstruct(id: number) {
	setLoader("ADD", "deleteNetworkConstruct");
	let api = new NetworkConstructApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.networkConstructDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_NETWORK_CONSTRUCT", payload: rtn });
	setLoader("REMOVE", "deleteNetworkConstruct");
	return rtn;
}

export async function DeleteDeepNetworkConstruct(id: number) {
	let api = new NetworkConstructApi();
	setLoader("ADD", "DeleteDeepNetworkConstruct");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.networkConstructDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_NETWORK_CONSTRUCT", payload: rtn });
	setLoader("REMOVE", "DeleteDeepNetworkConstruct");
	return rtn;
}

export async function GetRelatedRecordsNetworkConstruct(id: number) {
	let api = new NetworkConstructApi();
	setLoader("ADD", "GetRelatedRecordsNetworkConstruct");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.networkConstructGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsNetworkConstruct");
	return rtn;
}
