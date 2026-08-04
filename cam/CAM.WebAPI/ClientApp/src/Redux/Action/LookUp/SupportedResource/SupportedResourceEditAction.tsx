import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SupportedResourceApi } from "../../../../Business/LookUp/SupportedResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSupportedResourceEditResource(id: number) {
	setLoader("ADD", "GetSupportedResourceEditResource");

	let api = new SupportedResourceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.supportedResourceGetUpdateResourceSupportedResource(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_SUPPORTED_RESOURCE", payload: rtn });
	setLoader("REMOVE", "GetSupportedResourceEditResource");

	return rtn;
}

export async function EditSupportedResource(data: TipologicaGridDto) {
	setLoader("ADD", "EditSupportedResource");
	let api = new SupportedResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.supportedResourcePutSupportedResource(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_SUPPORTED_RESOURCE", payload: rtn });
	setLoader("REMOVE", "EditSupportedResource");
	return rtn;
}
