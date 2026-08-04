import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ResponsibilityPhaseApi } from "../../../../Business/LookUp/ResponsibilityPhaseBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetResponsibilityPhaseCreateResource() {
	setLoader("ADD", "GetResponsibilityPhaseCreateResource");

	let api = new ResponsibilityPhaseApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.responsibilityPhaseGetCreateResourceResponsibilityPhase());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_RESPONSABILITY_PHASE", payload: rtn });
	setLoader("REMOVE", "GetResponsibilityPhaseCreateResource");
}

export async function CreatResponsibilityPhase(data: TipologicaGridDto) {
	let api = new ResponsibilityPhaseApi();
	setLoader("ADD", "CreatResponsibilityPhase");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.responsibilityPhaseCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_RESPONSABILITY_PHASE", payload: rtn });
	setLoader("REMOVE", "CreatResponsibilityPhase");
	return rtn;
}
