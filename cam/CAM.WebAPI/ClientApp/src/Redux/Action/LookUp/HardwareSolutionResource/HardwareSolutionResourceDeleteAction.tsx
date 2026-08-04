import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { HardwareSolutionResourceApi } from "../../../../Business/LookUp/HardwareSolutionResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteHardwareSolutionResource(id: number) {
	setLoader("ADD", "deleteHardwareSolutionResource");
	let api = new HardwareSolutionResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.hardwareSolutionResourceDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_HARDWARE_SOLUTION_RESOURCE", payload: rtn });
	setLoader("REMOVE", "deleteHardwareSolutionResource");
	return rtn;
}

export async function DeleteDeepHardwareSolutionResource(id: number) {
	let api = new HardwareSolutionResourceApi();
	setLoader("ADD", "DeleteDeepHardwareSolutionResource");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.hardwareSolutionResourceDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_HARDWARE_SOLUTION_RESOURCE", payload: rtn });
	setLoader("REMOVE", "DeleteDeepHardwareSolutionResource");
	return rtn;
}

export async function GetRelatedRecordsHardwareSolutionResource(id: number) {
	let api = new HardwareSolutionResourceApi();
	setLoader("ADD", "GetRelatedRecordsHardwareSolutionResource");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.hardwareSolutionResourceGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsHardwareSolutionResource");
	return rtn;
}
