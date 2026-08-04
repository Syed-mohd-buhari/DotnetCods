import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SupportedResourceApi } from "../../../../Business/LookUp/SupportedResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSupportedResourceCreateResource() {
	setLoader("ADD", "GetSupportedResourceCreateResource");

	let api = new SupportedResourceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.supportedResourceGetCreateResourceSupportedResource());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_SUPPORTED_RESOURCE", payload: rtn });
	setLoader("REMOVE", "GetSupportedResourceCreateResource");
}

export async function CreatSupportedResource(data: TipologicaGridDto) {
	setLoader("ADD", "CreatSupportedResource");

	let api = new SupportedResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.supportedResourceCreateSupportedResource(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_SUPPORTED_RESOURCE", payload: rtn });
	setLoader("REMOVE", "CreatSupportedResource");
	return rtn;
}
