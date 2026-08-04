import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DeliveryStatusApi } from "../../../../Business/LookUp/DeliveryStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteDeliveryStatus(id: number) {
	setLoader("ADD", "deleteDeliveryStatus");
	let api = new DeliveryStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deliveryStatusDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_DELIVERY_STATUS", payload: rtn });
	setLoader("REMOVE", "deleteDeliveryStatus");
	return rtn;
}

export async function DeleteDeepDeliveryStatus(id: number) {
	let api = new DeliveryStatusApi();
	setLoader("ADD", "DeleteDeepDeliveryStatus");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deliveryStatusDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_DELIVERY_STATUS", payload: rtn });
	setLoader("REMOVE", "DeleteDeepDeliveryStatus");
	return rtn;
}

export async function GetRelatedRecordsDeliveryStatus(id: number) {
	let api = new DeliveryStatusApi();
	setLoader("ADD", "GetRelatedRecordsDeliveryStatus");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deliveryStatusGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsDeliveryStatus");
	return rtn;
}
