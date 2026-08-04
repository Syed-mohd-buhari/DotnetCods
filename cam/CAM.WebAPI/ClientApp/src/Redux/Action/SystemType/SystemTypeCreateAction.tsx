import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { SystemTypeApiFetchParamCreator, SystemTypeApi } from "../../../Business/SystemTypeBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { GET_CREATE_SYSTEM_TYPE, CREATE_SYSTEM_TYPE, SystemTypeCreate, SystemTypeDtoCreate } from "../../../Model/SystemTypeModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSystemTypeCreateResource() {
	setLoader("ADD", "GetSystemTypeCreateResource");

	let api = new SystemTypeApi();
	let createResource = await ApiCallWithErrorHandling<Promise<SystemTypeDtoCreate>>(() => api.systemTypeGetCreateResourceSystemType());
	let rtn = { ResultDtoCreate: null, SystemTypeDtoCreate: createResource } as SystemTypeCreate;
	rootStore.dispatch({ type: GET_CREATE_SYSTEM_TYPE, payload: rtn });
	setLoader("REMOVE", "GetSystemTypeCreateResource");

	return rtn.SystemTypeDtoCreate;
}

export async function CreatSystemType(data: SystemTypeDtoCreate, forced?: boolean) {
	setLoader("ADD", "CreatSystemType");
	let api = new SystemTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.systemTypeCreate(data, forced));
	let rtn = { ResultDtoCreate: result, SystemTypeDtoCreate: null } as SystemTypeCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: CREATE_SYSTEM_TYPE, payload: rtn });
	setLoader("REMOVE", "CreatSystemType");
	return rtn;
}
