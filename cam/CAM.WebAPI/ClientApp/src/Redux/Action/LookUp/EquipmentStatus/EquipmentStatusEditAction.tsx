import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { EquipmentStatusApi } from "../../../../Business/LookUp/EquipmentStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetEquipmentStatusEditResource(id: number) {
	setLoader("ADD", "GetEquipmentStatusEditResource");

	let api = new EquipmentStatusApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.equipmentStatusGetUpdateResourceEndOfSupportContract(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_EQUIPMENT_STATUS", payload: rtn });
	setLoader("REMOVE", "GetEquipmentStatusEditResource");

	return rtn;
}

export async function EditEquipmentStatus(data: TipologicaGridDto) {
	setLoader("ADD", "EditEquipmentStatus");
	let api = new EquipmentStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.equipmentStatusPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_EQUIPMENT_STATUS", payload: rtn });
	setLoader("REMOVE", "EditEquipmentStatus");
	return rtn;
}
