import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DeliveryStatusApi } from "../../../../Business/LookUp/DeliveryStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDtoCombinationRule } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetDeliveryStatusEditResource(id: number) {
	setLoader("ADD", "GetDeliveryStatusEditResource");

	let api = new DeliveryStatusApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDtoCombinationRule>>(() => api.deliveryStatusGetUpdateResourceDeliveryStatus(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_DELIVERY_STATUS", payload: rtn });
	setLoader("REMOVE", "GetDeliveryStatusEditResource");

	return rtn;
}

export async function EditDeliveryStatus(data: TipologicaGridDtoCombinationRule) {
	setLoader("ADD", "EditDeliveryStatus");
	let api = new DeliveryStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deliveryStatusPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDtoCombinationRule;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_DELIVERY_STATUS", payload: rtn });
	setLoader("REMOVE", "EditDeliveryStatus");
	return rtn;
}
