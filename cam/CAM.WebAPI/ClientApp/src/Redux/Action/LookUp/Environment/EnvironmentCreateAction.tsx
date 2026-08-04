import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { EnvironmentApi } from "../../../../Business/LookUp/EnvironmentBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetEnvironmentCreateResource() {
	setLoader("REMOVE", "GetEnvironmentCreateResource");

	let api = new EnvironmentApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.environmentGetCreateResourceEnvironment());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_ENVIRONMENT", payload: rtn });
	setLoader("REMOVE", "GetEnvironmentCreateResource");
}

export async function CreatEnvironment(data: TipologicaGridDto) {
	setLoader("ADD", "CreatEnvironment");
	let api = new EnvironmentApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.environmentCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_ENVIRONMENT", payload: rtn });
	setLoader("REMOVE", "CreatEnvironment");
	return rtn;
}
