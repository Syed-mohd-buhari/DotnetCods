import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ReasonCheckboxResourceApi } from "../../../../Business/LookUp/ReasonCheckboxBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { ReasonCheckboxDto } from "../../../../Model/LookUp/ReasonCheckbox";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetReasonCheckboxEditResource(id: number) {
	setLoader("ADD", "GetReasonCheckboxEditResource");

	let api = new ReasonCheckboxResourceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<ReasonCheckboxDto>>(() => api.reasonCheckboxResourceGetUpdateResourceReasonCheckboxResource(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_REASON_CHECKBOX", payload: rtn });
	setLoader("REMOVE", "GetReasonCheckboxEditResource");

	return rtn;
}
export async function EditReasonCheckbox(data: TipologicaGridDto) {
	setLoader("ADD", "EditReasonCheckbox");
	let api = new ReasonCheckboxResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.reasonCheckboxResourcePut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_REASON_CHECKBOX", payload: rtn });
	setLoader("REMOVE", "EditReasonCheckbox");
	return rtn;
}
