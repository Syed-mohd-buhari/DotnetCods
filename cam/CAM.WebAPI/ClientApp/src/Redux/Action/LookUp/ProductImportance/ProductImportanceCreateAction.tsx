import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ProductImportanceApi } from "../../../../Business/LookUp/ProductImportanceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetProductImportanceCreateResource() {
	setLoader("ADD", "GetProductImportanceCreateResource");

	let api = new ProductImportanceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.productImportanceGetCreateResourceProductImportance());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_PRODUCT_IMPORTANCE", payload: rtn });
	setLoader("REMOVE", "GetProductImportanceCreateResource");
}

export async function CreatProductImportance(data: TipologicaGridDto) {
	setLoader("ADD", "CreatProductImportance");
	let api = new ProductImportanceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.productImportanceCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_PRODUCT_IMPORTANCE", payload: rtn });
	setLoader("REMOVE", "CreatProductImportance");
	return rtn;
}
