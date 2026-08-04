import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ProductImportanceApi } from "../../../../Business/LookUp/ProductImportanceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteProductImportance(id: number) {
	setLoader("ADD", "deleteProductImportance");
	let api = new ProductImportanceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.productImportanceDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_PRODUCT_IMPORTANCE", payload: rtn });
	setLoader("REMOVE", "deleteProductImportance");
	return rtn;
}

export async function DeleteDeepProductImportance(id: number) {
	let api = new ProductImportanceApi();
	setLoader("ADD", "DeleteDeepProductImportance");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.productImportanceDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_PRODUCT_IMPORTANCE", payload: rtn });
	setLoader("REMOVE", "DeleteDeepProductImportance");
	return rtn;
}

export async function GetRelatedRecordsProductImportance(id: number) {
	let api = new ProductImportanceApi();
	setLoader("ADD", "GetRelatedRecordsProductImportance");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.productImportanceGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsProductImportance");
	return rtn;
}
