import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DeliveryStatusApi } from "../../../../Business/LookUp/DeliveryStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, LookUpCreateRule, TipologicaGridDtoCombinationRule } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetDeliveryStatusCreateResource() {
	setLoader("ADD", "GetDeliveryStatusCreateResource");

	let api = new DeliveryStatusApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDtoCombinationRule>>(() => api.deliveryStatusGetCreateResourceDeliveryStatus());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreateRule;
	rootStore.dispatch({ type: "GET_CREATE_DELIVERY_STATUS", payload: rtn });
	setLoader("REMOVE", "GetDeliveryStatusCreateResource");
}

export async function CreatDeliveryStatus(data: TipologicaGridDtoCombinationRule) {
	let api = new DeliveryStatusApi();
	setLoader("ADD", "CreatDeliveryStatus");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deliveryStatusCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreateRule;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_DELIVERY_STATUS", payload: rtn });
	setLoader("REMOVE", "CreatDeliveryStatus");
	return rtn;
}
