import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { LocationApi } from "../../../../Business/LookUp/LocationBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import { LocationCreate, LocationDto } from '../../../../Model/LookUp/Location';

export async function GetLocationCreateResource() {
	setLoader("ADD", "GetLocationCreateResource");

	let api = new LocationApi();
	let createResource = await ApiCallWithErrorHandling<Promise<LocationDto>>(() => api.locationGetCreateResourceLocation());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LocationCreate;
	rootStore.dispatch({ type: "GET_CREATE_LOCATION", payload: rtn });
	setLoader("REMOVE", "GetLocationCreateResource");
}

export async function CreatLocation(data: LocationDto) {
	setLoader("ADD", "CreatLocation");
	let api = new LocationApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.locationCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LocationCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_LOCATION", payload: rtn });
	setLoader("REMOVE", "CreatLocation");
	return rtn;
}
