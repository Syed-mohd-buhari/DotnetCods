import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ReasonCheckboxResourceApi } from "../../../../Business/LookUp/ReasonCheckboxBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { ReasonCheckboxDto } from "../../../../Model/LookUp/ReasonCheckbox";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetReasonCheckboxCreateResource() {
	setLoader("ADD", "GetReasonCheckboxCreateResource");

	let api = new ReasonCheckboxResourceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<ReasonCheckboxDto>>(() => api.reasonCheckboxResourceGetCreateResourceReasonCheckboxResource());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_REASON_CHECKBOX", payload: rtn });
	setLoader("REMOVE", "GetReasonCheckboxCreateResource");
}

export async function CreatReasonCheckbox(data: TipologicaGridDto) {
	setLoader("ADD", "CreatReasonCheckbox");
	let api = new ReasonCheckboxResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.reasonCheckboxResourceCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_REASON_CHECKBOX", payload: rtn });
	setLoader("REMOVE", "CreatReasonCheckbox");
	return rtn;
}
