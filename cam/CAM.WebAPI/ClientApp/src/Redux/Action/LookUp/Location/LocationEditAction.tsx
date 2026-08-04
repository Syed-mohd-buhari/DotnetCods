import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { LocationApi } from "../../../../Business/LookUp/LocationBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LocationDto, LocationDtoGrid, LocationEdit } from "../../../../Model/LookUp/Location";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetLocationEditResource(id: number) {
	setLoader("ADD", "GetLocationEditResource");

	let api = new LocationApi();
	let createResource = await ApiCallWithErrorHandling<Promise<LocationDto>>(() => api.locationGetUpdateResourceLocation(id));
	let rtn = { LookUpDtoEdit: createResource } as LocationEdit;
	rootStore.dispatch({ type: "GET_EDIT_LOCATION", payload: rtn });
	setLoader("REMOVE", "GetLocationEditResource");

	return rtn;
}

export async function EditLocation(data: LocationDto) {
	setLoader("ADD", "EditLocation");
	let api = new LocationApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.locationPut(data));
	let rtn = { ResultDtoEdit: result } as LocationEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_LOCATION", payload: rtn });
	setLoader("REMOVE", "EditLocation");
	return rtn;
}
