import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ComponentManufacturerApi } from "../../../../Business/LookUp/ComponentManufactureBuisness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetCompoentManufacturerEditResource(id: number) {
	setLoader("ADD", "GetOriginalEquipmentManufacturerEditResource");

	let api = new ComponentManufacturerApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.componentManufacturerGetUpdateResourceOriginalEquipmentManufacturer(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_COMPONENT_MANUFACTURER", payload: rtn });
	setLoader("REMOVE", "GetOriginalEquipmentManufacturerEditResource");

	return rtn;
}

export async function EditComponentManufacturer(data: TipologicaGridDto) {
	let api = new ComponentManufacturerApi();
	setLoader("ADD", "EditOriginalEquipmentManufacturer");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.componentManufacturerPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_COMPONENT_MANUFACTURER", payload: rtn });
	setLoader("REMOVE", "EditOriginalEquipmentManufacturer");
	return rtn;
}
