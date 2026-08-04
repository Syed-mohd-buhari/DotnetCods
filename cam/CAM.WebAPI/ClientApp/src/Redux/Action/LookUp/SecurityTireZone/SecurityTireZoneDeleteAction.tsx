import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SecurityTireZoneApi } from "../../../../Business/LookUp/SecurityTireZoneBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteSecurityTireZone(id: number) {
	setLoader("ADD", "deleteSecurityTireZone");
	let api = new SecurityTireZoneApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.securityTireZoneDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_SECURITY_TIRE_ZONE", payload: rtn });
	setLoader("REMOVE", "deleteSecurityTireZone");
	return rtn;
}

export async function DeleteDeepSecurityTireZone(id: number) {
	let api = new SecurityTireZoneApi();
	setLoader("ADD", "DeleteDeepSecurityTireZone");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.securityTireZoneDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_SECURITY_TIRE_ZONE", payload: rtn });
	setLoader("REMOVE", "DeleteDeepSecurityTireZone");
	return rtn;
}

export async function GetRelatedRecordsSecurityTireZone(id: number) {
	let api = new SecurityTireZoneApi();
	setLoader("ADD", "GetRelatedRecordsSecurityTireZone");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.securityTireZoneGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsSecurityTireZone");
	return rtn;
}
