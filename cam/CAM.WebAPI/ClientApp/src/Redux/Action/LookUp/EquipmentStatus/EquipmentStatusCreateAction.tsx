import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { EquipmentStatusApi } from "../../../../Business/LookUp/EquipmentStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetEquipmentStatusCreateResource() {
	setLoader("ADD", "GetEquipmentStatusCreateResource");

	let api = new EquipmentStatusApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.equipmentStatusGetCreateResourceEndOfSupportContract());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_EQUIPMENT_STATUS", payload: rtn });
	setLoader("REMOVE", "GetEquipmentStatusCreateResource");
}

export async function CreatEquipmentStatus(data: TipologicaGridDto) {
	setLoader("ADD", "CreatEquipmentStatus");
	let api = new EquipmentStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.equipmentStatusCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_EQUIPMENT_STATUS", payload: rtn });
	setLoader("REMOVE", "CreatEquipmentStatus");
	return rtn;
}
