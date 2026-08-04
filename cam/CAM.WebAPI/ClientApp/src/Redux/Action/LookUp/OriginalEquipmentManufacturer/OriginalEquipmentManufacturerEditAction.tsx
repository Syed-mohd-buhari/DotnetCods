import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { OriginalEquipmentManufacturerApi } from "../../../../Business/LookUp/OriginalEquipmentManufacturerBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetOriginalEquipmentManufacturerEditResource(id: number) {
	setLoader("ADD", "GetOriginalEquipmentManufacturerEditResource");

	let api = new OriginalEquipmentManufacturerApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.originalEquipmentManufacturerGetUpdateResourceOriginalEquipmentManufacturer(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_ORIGINAL_EQUIPMENT_MANUFACTURER", payload: rtn });
	setLoader("REMOVE", "GetOriginalEquipmentManufacturerEditResource");

	return rtn;
}

export async function EditOriginalEquipmentManufacturer(data: TipologicaGridDto) {
	let api = new OriginalEquipmentManufacturerApi();
	setLoader("ADD", "EditOriginalEquipmentManufacturer");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.originalEquipmentManufacturerPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_ORIGINAL_EQUIPMENT_MANUFACTURER", payload: rtn });
	setLoader("REMOVE", "EditOriginalEquipmentManufacturer");
	return rtn;
}
