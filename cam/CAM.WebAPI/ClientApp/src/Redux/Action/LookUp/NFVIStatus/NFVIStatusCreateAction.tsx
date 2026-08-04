import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NFVIStatusApi } from "../../../../Business/LookUp/NFVIStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetNFVIStatusCreateResource() {
	setLoader("ADD", "GetNFVIStatusCreateResource");

	let api = new NFVIStatusApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.nFVIStatusGetCreateResourceNFVIStatus());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_NFVI_STATUS", payload: rtn });
	setLoader("REMOVE", "GetNFVIStatusCreateResource");
}

export async function CreatNFVIStatus(data: TipologicaGridDto) {
	setLoader("ADD", "CreatNFVIStatus");
	let api = new NFVIStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVIStatusCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_NFVI_STATUS", payload: rtn });
	setLoader("REMOVE", "CreatNFVIStatus");
	return rtn;
}
