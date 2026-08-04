import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SupportProviderApi } from "../../../../Business/LookUp/SupportProviderBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSupportProviderCreateResource() {
	setLoader("ADD", "GetSupportProviderCreateResource");

	let api = new SupportProviderApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.supportProviderGetCreateResourceSupportProvider());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_SUPPORT_PROVIDER", payload: rtn });
	setLoader("REMOVE", "GetSupportProviderCreateResource");
}

export async function CreatSupportProvider(data: TipologicaGridDto) {
	setLoader("ADD", "CreatSupportProvider");
	let api = new SupportProviderApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.supportProviderCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_SUPPORT_PROVIDER", payload: rtn });
	setLoader("REMOVE", "CreatSupportProvider");
	return rtn;
}
