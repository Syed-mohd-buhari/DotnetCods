import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SubDomainSpocApi } from "../../../../Business/LookUp/SubDomainSpocBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteSubDomainSpoc(id: number) {
	setLoader("ADD", "deleteSubDomainSpoc");
	let api = new SubDomainSpocApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.subDomainSpocDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_SUB_DOMAIN_SPOC", payload: rtn });
	setLoader("REMOVE", "deleteSubDomainSpoc");
	return rtn;
}

export async function DeleteDeepSubDomainSpoc(id: number) {
	let api = new SubDomainSpocApi();
	setLoader("ADD", "DeleteDeepSubDomainSpoc");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.subDomainSpocDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_SUB_DOMAIN_SPOC", payload: rtn });
	setLoader("REMOVE", "DeleteDeepSubDomainSpoc");
	return rtn;
}

export async function GetRelatedRecordsSubDomainSpoc(id: number) {
	let api = new SubDomainSpocApi();
	setLoader("ADD", "GetRelatedRecordsSubDomainSpoc");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.subDomainSpocGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsSubDomainSpoc");
	return rtn;
}
