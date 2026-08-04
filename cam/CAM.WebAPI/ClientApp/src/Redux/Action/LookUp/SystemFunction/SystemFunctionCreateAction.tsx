import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SystemFunctionApi } from "../../../../Business/LookUp/SystemFunctionBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSystemFunctionCreateResource() {
	setLoader("ADD", "GetSystemFunctionCreateResource");

	let api = new SystemFunctionApi();

	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.systemFunctionGetCreateResourceSystemFunction());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_SYSTEM_FUNCTION", payload: rtn });
	setLoader("REMOVE", "GetSystemFunctionCreateResource");
}

export async function CreatSystemFunction(data: TipologicaGridDto) {
	setLoader("ADD", "CreatSystemFunction");
	let api = new SystemFunctionApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.systemFunctionCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_SYSTEM_FUNCTION", payload: rtn });
	setLoader("REMOVE", "CreatSystemFunction");
	return rtn;
}
