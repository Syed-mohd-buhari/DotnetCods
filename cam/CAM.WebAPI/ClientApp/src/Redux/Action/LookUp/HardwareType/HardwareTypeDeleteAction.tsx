import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { HardwareTypeApi } from "../../../../Business/LookUp/HardwareTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteHardwareType(id: number) {
	setLoader("ADD", "deleteHardwareType");
	let api = new HardwareTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.hardwareTypeDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_HARDWARE_TYPE", payload: rtn });
	setLoader("REMOVE", "deleteHardwareType");
	return rtn;
}

export async function DeleteDeepHardwareType(id: number) {
	let api = new HardwareTypeApi();
	setLoader("ADD", "DeleteDeepHardwareType");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.hardwareTypeDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_HARDWARE_TYPE", payload: rtn });
	setLoader("REMOVE", "DeleteDeepHardwareType");
	return rtn;
}

export async function GetRelatedRecordsHardwareType(id: number) {
	let api = new HardwareTypeApi();
	setLoader("ADD", "GetRelatedRecordsHardwareType");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.hardwareTypeGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsHardwareType");
	return rtn;
}
