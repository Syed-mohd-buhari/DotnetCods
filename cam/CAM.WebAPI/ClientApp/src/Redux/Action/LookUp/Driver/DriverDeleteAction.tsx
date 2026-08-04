import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DriverApi } from "../../../../Business/LookUp/DriverBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteDriver(id: number) {
	setLoader("ADD", "deleteDriver");
	let api = new DriverApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.driverDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_DRIVER", payload: rtn });
	setLoader("REMOVE", "deleteDriver");
	return rtn;
}

export async function DeleteDeepDriver(id: number) {
	let api = new DriverApi();
	setLoader("ADD", "DeleteDeepDriver");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.driverDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_DRIVER", payload: rtn });
	setLoader("REMOVE", "DeleteDeepDriver");
	return rtn;
}

export async function GetRelatedRecordsDriver(id: number) {
	let api = new DriverApi();
	setLoader("ADD", "GetRelatedRecordsDriver");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.driverGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsDriver");
	return rtn;
}
