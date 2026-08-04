import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ComponentManufacturerApi } from "../../../../Business/LookUp/ComponentManufactureBuisness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function deleteComponentManufacturer(id: number) {
	setLoader("ADD", "deleteComponentManufacturer");
	let api = new ComponentManufacturerApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.componentManufacturerDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_COMPONENT_MANUFACTURER", payload: rtn });
	setLoader("REMOVE", "deleteComponentManufacturer");
	return rtn;
}

export async function DeleteDeepComponentManufacturer(id: number) {
	let api = new ComponentManufacturerApi();
	setLoader("ADD", "DeleteDeepComponentManufacturer");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.componentManufacturerDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_COMPONENT_MANUFACTURER", payload: rtn });
	setLoader("REMOVE", "DeleteDeepComponentManufacturer");
	return rtn;
}

export async function GetRelatedRecordsComponentManufacturer(id: number) {
	let api = new ComponentManufacturerApi();
	setLoader("ADD", "GetRelatedRecordsComponentManufacturer");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.componentManufacturerGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsComponentManufacturer");
	return rtn;
}
