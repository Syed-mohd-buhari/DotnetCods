import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SecurityTireZoneApi } from "../../../../Business/LookUp/SecurityTireZoneBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSecurityTireZoneEditResource(id: number) {
	setLoader("ADD", "GetSecurityTireZoneEditResource");

	let api = new SecurityTireZoneApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.securityTireZoneGetUpdateResourceSecurityTireZone(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_SECURITY_TIRE_ZONE", payload: rtn });
	setLoader("REMOVE", "GetSecurityTireZoneEditResource");

	return rtn;
}

export async function EditSecurityTireZone(data: TipologicaGridDto) {
	setLoader("ADD", "EditSecurityTireZone");
	let api = new SecurityTireZoneApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.securityTireZonePut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_SECURITY_TIRE_ZONE", payload: rtn });
	setLoader("REMOVE", "EditSecurityTireZone");
	return rtn;
}
