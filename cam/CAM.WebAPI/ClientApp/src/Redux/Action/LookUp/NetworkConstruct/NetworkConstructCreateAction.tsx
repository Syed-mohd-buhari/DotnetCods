import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NetworkConstructApi } from "../../../../Business/LookUp/NetworkConstructBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetNetworkConstructCreateResource() {
	setLoader("ADD", "GetNetworkConstructCreateResource");

	let api = new NetworkConstructApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.networkConstructGetCreateResourceNetworkConstruct());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_NETWORK_CONSTRUCT", payload: rtn });
	setLoader("REMOVE", "GetNetworkConstructCreateResource");
}

export async function CreatNetworkConstruct(data: TipologicaGridDto) {
	setLoader("ADD", "CreatNetworkConstruct");
	let api = new NetworkConstructApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.networkConstructCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_NETWORK_CONSTRUCT", payload: rtn });
	setLoader("REMOVE", "CreatNetworkConstruct");
	return rtn;
}
