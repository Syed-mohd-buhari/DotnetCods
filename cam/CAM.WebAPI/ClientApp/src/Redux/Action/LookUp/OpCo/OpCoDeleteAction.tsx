import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { OpCoApi } from "../../../../Business/LookUp/OpCoBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteOpCo(id: number) {
	setLoader("ADD", "deleteOpCo");
	let api = new OpCoApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.opCoDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_OP_CO", payload: rtn });
	setLoader("REMOVE", "deleteOpCo");
	return rtn;
}

export async function DeleteDeepOpCo(id: number) {
	let api = new OpCoApi();
	setLoader("ADD", "DeleteDeepOpCo");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.opCoDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_OP_CO", payload: rtn });
	setLoader("REMOVE", "DeleteDeepOpCo");
	return rtn;
}

export async function GetRelatedRecordsOpCo(id: number) {
	let api = new OpCoApi();
	setLoader("ADD", "GetRelatedRecordsOpCo");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.opCoGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsOpCo");
	return rtn;
}
