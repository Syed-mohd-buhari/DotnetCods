import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { LocationApi } from "../../../../Business/LookUp/LocationBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteLocation(id: number) {
	setLoader("ADD", "deleteLocation");
	let api = new LocationApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.locationDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_LOCATION", payload: rtn });
	setLoader("REMOVE", "deleteLocation");
	return rtn;
}

export async function DeleteDeepLocation(id: number) {
	let api = new LocationApi();
	setLoader("ADD", "DeleteDeepLocation");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.locationDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_LOCATION", payload: rtn });
	setLoader("REMOVE", "DeleteDeepLocation");
	return rtn;
}

export async function GetRelatedRecordsLocation(id: number) {
	let api = new LocationApi();
	setLoader("ADD", "GetRelatedRecordsLocation");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.locationGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsLocation");
	return rtn;
}
