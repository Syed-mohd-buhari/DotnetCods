import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DriverApi } from "../../../../Business/LookUp/ProgramBuisness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteProgram(id: number) {
	setLoader("ADD", "deleteProgram");
	let api = new DriverApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.programDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_PROGRAM", payload: rtn });
	setLoader("REMOVE", "deleteProgram");
	return rtn;
}

export async function DeleteDeepProgram(id: number) {
	let api = new DriverApi();
	setLoader("ADD", "DeleteDeepProgram");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.programDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_PROGRAM", payload: rtn });
	setLoader("REMOVE", "DeleteDeepProgram");
	return rtn;
}

export async function GetRelatedRecordsProgram(id: number) {
	let api = new DriverApi();
	setLoader("ADD", "GetRelatedRecordsProgram");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.programGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsProgram");
	return rtn;
}
