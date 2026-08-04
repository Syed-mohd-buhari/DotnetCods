import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { VolteKPIApiFetchParamCreator, VolteKPIApi } from "../../../Business/VolteKPIBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { VolteKPIDtoUpdate, VolteKPIEdit, EDIT_VOLTE_KPI, GET_EDIT_VOLTE_KPI } from "../../../Model/VolteKpi/VolteKPI";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetVolteKPIEditResource(id: number) {
	setLoader("ADD", "GetVolteKPIEditResource");

	// const dispach = useDispatch();
	let api = new VolteKPIApi();
	let createResource = await ApiCallWithErrorHandling<Promise<VolteKPIDtoUpdate>>(() => api.volteKPIGetUpdateResourceVolteKPI(id));
	let rtn = { VolteKPIDtoEdit: createResource } as VolteKPIEdit;
	rootStore.dispatch({ type: GET_EDIT_VOLTE_KPI, payload: rtn });
	setLoader("REMOVE", "GetVolteKPIEditResource");

	return rtn;
}

export async function EditVolteKPI(data: VolteKPIDtoUpdate, forced?: boolean) {
	setLoader("ADD", "EditVolteKPI");
	let api = new VolteKPIApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.volteKPIPut(data, forced));
	let rtn = { ResultDtoEdit: result } as VolteKPIEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: EDIT_VOLTE_KPI, payload: rtn });
	setLoader("REMOVE", "EditVolteKPI");
	return rtn;
}
