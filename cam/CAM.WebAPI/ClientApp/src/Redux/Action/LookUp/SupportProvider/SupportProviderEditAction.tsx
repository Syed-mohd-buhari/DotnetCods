import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SupportProviderApi } from "../../../../Business/LookUp/SupportProviderBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSupportProviderEditResource(id: number) {
	setLoader("ADD", "GetSupportProviderEditResource");

	let api = new SupportProviderApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.supportProviderGetUpdateResourceSupportProvider(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_SUPPORT_PROVIDER", payload: rtn });
	setLoader("REMOVE", "GetSupportProviderEditResource");

	return rtn;
}

export async function EditSupportProvider(data: TipologicaGridDto) {
	setLoader("ADD", "EditSupportProvider");
	let api = new SupportProviderApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.supportProviderPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_SUPPORT_PROVIDER", payload: rtn });
	setLoader("REMOVE", "EditSupportProvider");
	return rtn;
}
