import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SubDomainSpocApi } from "../../../../Business/LookUp/SubDomainSpocBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSubDomainSpocCreateResource() {
	setLoader("ADD", "GetSubDomainSpocCreateResource");

	let api = new SubDomainSpocApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.subDomainSpocGetCreateResourceSubDomainSpoc());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_SUB_DOMAIN_SPOC", payload: rtn });
	setLoader("REMOVE", "GetSubDomainSpocCreateResource");
}

export async function CreatSubDomainSpoc(data: TipologicaGridDto) {
	setLoader("ADD", "CreatSubDomainSpoc");
	let api = new SubDomainSpocApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.subDomainSpocCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_SUB_DOMAIN_SPOC", payload: rtn });
	setLoader("REMOVE", "CreatSubDomainSpoc");
	return rtn;
}
