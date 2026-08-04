import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { RelatesToResourceApi } from "../../../../Business/LookUp/RelatesToResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetRelatesToResourceCreateResource() {
	setLoader("ADD", "GetRelatesToResourceCreateResource");

	let api = new RelatesToResourceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.relatesToResourceGetCreateResourceRelatesToResource());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_RELATES_TO_RESOURCE", payload: rtn });
	setLoader("REMOVE", "GetRelatesToResourceCreateResource");
}

export async function CreatRelatesToResource(data: TipologicaGridDto) {
	setLoader("ADD", "CreatRelatesToResource");
	let api = new RelatesToResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.relatesToResourceCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_RELATES_TO_RESOURCE", payload: rtn });
	setLoader("REMOVE", "CreatRelatesToResource");
	return rtn;
}
