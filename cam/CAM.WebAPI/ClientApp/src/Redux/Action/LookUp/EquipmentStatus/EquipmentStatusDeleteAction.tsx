import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { EquipmentStatusApi } from "../../../../Business/LookUp/EquipmentStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteEquipmentStatus(id: number) {
	setLoader("ADD", "deleteEquipmentStatus");
	let api = new EquipmentStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.equipmentStatusDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_EQUIPMENT_STATUS", payload: rtn });
	setLoader("REMOVE", "deleteEquipmentStatus");
	return rtn;
}

export async function DeleteDeepEquipmentStatus(id: number) {
	let api = new EquipmentStatusApi();
	setLoader("ADD", "DeleteDeepEquipmentStatus");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.equipmentStatusDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_EQUIPMENT_STATUS", payload: rtn });
	setLoader("REMOVE", "DeleteDeepEquipmentStatus");
	return rtn;
}

export async function GetRelatedRecordsEquipmentStatus(id: number) {
	let api = new EquipmentStatusApi();
	setLoader("ADD", "GetRelatedRecordsEquipmentStatus");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.equipmentStatusGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsEquipmentStatus");
	return rtn;
}
