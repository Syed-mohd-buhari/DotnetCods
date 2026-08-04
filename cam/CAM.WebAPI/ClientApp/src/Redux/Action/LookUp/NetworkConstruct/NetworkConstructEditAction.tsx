import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NetworkConstructApi } from "../../../../Business/LookUp/NetworkConstructBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetNetworkConstructEditResource(id: number) {
	setLoader("ADD", "GetNetworkConstructEditResource");

	let api = new NetworkConstructApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.networkConstructGetUpdateResourceNetworkConstruct(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_NETWORK_CONSTRUCT", payload: rtn });
	setLoader("REMOVE", "GetNetworkConstructEditResource");

	return rtn;
}

export async function EditNetworkConstruct(data: TipologicaGridDto) {
	setLoader("ADD", "EditNetworkConstruct");
	let api = new NetworkConstructApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.networkConstructPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_NETWORK_CONSTRUCT", payload: rtn });
	setLoader("REMOVE", "EditNetworkConstruct");
	return rtn;
}
