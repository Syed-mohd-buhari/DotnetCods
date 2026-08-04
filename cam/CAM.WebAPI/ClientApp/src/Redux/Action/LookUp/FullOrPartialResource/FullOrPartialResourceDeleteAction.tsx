import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { FullOrPartialResourceApi } from "../../../../Business/LookUp/FullOrPartialResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteFullOrPartialResource(id: number) {
	setLoader("ADD", "deleteFullOrPartialResource");
	let api = new FullOrPartialResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.fullOrPartialResourceDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_F_O_P_R", payload: rtn });
	setLoader("REMOVE", "deleteFullOrPartialResource");
	return rtn;
}

export async function DeleteDeepFullOrPartialResource(id: number) {
	let api = new FullOrPartialResourceApi();
	setLoader("ADD", "DeleteDeepFullOrPartialResource");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.fullOrPartialResourceDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_F_O_P_R", payload: rtn });
	setLoader("REMOVE", "DeleteDeepFullOrPartialResource");
	return rtn;
}

export async function GetRelatedRecordsFullOrPartialResource(id: number) {
	let api = new FullOrPartialResourceApi();
	setLoader("ADD", "GetRelatedRecordsFullOrPartialResource");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.fullOrPartialResourceGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsFullOrPartialResource");
	return rtn;
}
