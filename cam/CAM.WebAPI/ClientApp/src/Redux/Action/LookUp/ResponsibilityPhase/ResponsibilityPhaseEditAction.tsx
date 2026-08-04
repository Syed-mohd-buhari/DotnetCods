import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ResponsibilityPhaseApi } from "../../../../Business/LookUp/ResponsibilityPhaseBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetResponsibilityPhaseEditResource(id: number) {
	setLoader("ADD", "GetResponsibilityPhaseEditResource");

	let api = new ResponsibilityPhaseApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.responsibilityPhaseGetUpdateResourceResponsibilityPhase(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_RESPONSABILITY_PHASE", payload: rtn });
	setLoader("REMOVE", "GetResponsibilityPhaseEditResource");

	return rtn;
}

export async function EditResponsibilityPhase(data: TipologicaGridDto) {
	setLoader("ADD", "EditResponsibilityPhase");
	let api = new ResponsibilityPhaseApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.responsibilityPhasePut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_RESPONSABILITY_PHASE", payload: rtn });
	setLoader("REMOVE", "EditResponsibilityPhase");
	return rtn;
}
