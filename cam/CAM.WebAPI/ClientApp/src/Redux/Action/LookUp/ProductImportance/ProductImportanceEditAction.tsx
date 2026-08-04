import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ProductImportanceApi } from "../../../../Business/LookUp/ProductImportanceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetProductImportanceEditResource(id: number) {
	setLoader("ADD", "GetProductImportanceEditResource");

	let api = new ProductImportanceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.productImportanceGetUpdateResourceProductImportance(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_PRODUCT_IMPORTANCE", payload: rtn });
	setLoader("REMOVE", "GetProductImportanceEditResource");

	return rtn;
}

export async function EditProductImportance(data: TipologicaGridDto) {
	setLoader("ADD", "EditProductImportance");
	let api = new ProductImportanceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.productImportancePut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_PRODUCT_IMPORTANCE", payload: rtn });
	setLoader("REMOVE", "EditProductImportance");
	return rtn;
}
