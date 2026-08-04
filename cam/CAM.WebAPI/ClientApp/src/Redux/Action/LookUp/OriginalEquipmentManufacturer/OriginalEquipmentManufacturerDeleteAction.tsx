import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { OriginalEquipmentManufacturerApi } from "../../../../Business/LookUp/OriginalEquipmentManufacturerBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function deleteOriginalEquipmentManufacturer(id: number) {
	setLoader("ADD", "deleteOriginalEquipmentManufacturer");
	let api = new OriginalEquipmentManufacturerApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.originalEquipmentManufacturerDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_ORIGINAL_EQUIPMENT_MANUFACTURER", payload: rtn });
	setLoader("REMOVE", "deleteOriginalEquipmentManufacturer");
	return rtn;
}

export async function DeleteDeepOriginalEquipmentManufacturer(id: number) {
	let api = new OriginalEquipmentManufacturerApi();
	setLoader("ADD", "DeleteDeepOriginalEquipmentManufacturer");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.originalEquipmentManufacturerDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_ORIGINAL_EQUIPMENT_MANUFACTURER", payload: rtn });
	setLoader("REMOVE", "DeleteDeepOriginalEquipmentManufacturer");
	return rtn;
}

export async function GetRelatedRecordsOriginalEquipmentManufacturer(id: number) {
	let api = new OriginalEquipmentManufacturerApi();
	setLoader("ADD", "GetRelatedRecordsOriginalEquipmentManufacturer");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.originalEquipmentManufacturerGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsOriginalEquipmentManufacturer");
	return rtn;
}
