import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlatformApi } from "../../../../Business/LookUp/PlatformBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetPlatformCreateResource() {
	setLoader("ADD", "GetPlatformCreateResource");

	let api = new PlatformApi();

	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.platformGetCreateResourcePlatform());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_PLATFORM", payload: rtn });
	setLoader("REMOVE", "GetPlatformCreateResource");
}

export async function CreatPlatform(data: TipologicaGridDto) {
	setLoader("ADD", "CreatPlatform");
	let api = new PlatformApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.platformCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_PLATFORM", payload: rtn });
	setLoader("REMOVE", "CreatPlatform");
	return rtn;
}
