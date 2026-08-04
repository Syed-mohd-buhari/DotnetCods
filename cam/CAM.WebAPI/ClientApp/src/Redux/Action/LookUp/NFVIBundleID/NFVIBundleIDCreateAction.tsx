import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NFVIBundleIDApi } from "../../../../Business/LookUp/NFVIBundleIDBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetNFVIBundleIDCreateResource() {
	setLoader("ADD", "GetNFVIBundleIDCreateResource");

	let api = new NFVIBundleIDApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.nFVIBundleIDGetCreateResourceActivityStatus());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_NFVI_BUNDLE_ID", payload: rtn });
	setLoader("REMOVE", "GetNFVIBundleIDCreateResource");
}

export async function CreatNFVIBundleID(data: TipologicaGridDto) {
	setLoader("ADD", "CreatNFVIBundleID");
	let api = new NFVIBundleIDApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVIBundleIDCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_NFVI_BUNDLE_ID", payload: rtn });
	setLoader("REMOVE", "CreatNFVIBundleID");
	return rtn;
}
