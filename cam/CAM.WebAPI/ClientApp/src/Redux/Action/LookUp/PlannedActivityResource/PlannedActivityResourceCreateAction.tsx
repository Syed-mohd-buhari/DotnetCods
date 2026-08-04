import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlannedActivityResourceApi } from "../../../../Business/LookUp/PlannedActivityResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { PlannedActivityResourceDto } from "../../../../Model/LookUp/PlannedActivityResource";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetPlannedActivityResourceCreateResource() {
	setLoader("ADD", "GetPlannedActivityResourceCreateResource");

	let api = new PlannedActivityResourceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.plannedActivityResourceGetCreateResourcePlannedActivityResource());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_PLANNED_ACTIVITY_RESOURCE", payload: rtn });
	setLoader("REMOVE", "GetPlannedActivityResourceCreateResource");
}

export async function CreatPlannedActivityResource(data: PlannedActivityResourceDto) {
	setLoader("ADD", "CreatPlannedActivityResource");
	let api = new PlannedActivityResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.plannedActivityResourceCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_PLANNED_ACTIVITY_RESOURCE", payload: rtn });
	setLoader("REMOVE", "CreatPlannedActivityResource");
	return rtn;
}

export async function GetPlannedActivityResourceForDropdown() {
	//Così quando chiudo la Lookup Passo un PlannedActivityResourceDto invece che PlannedActivityResourceDtoGrid alla modale di Network Element As Planned
	setLoader("ADD", "GetPlannedActivityResourceForDropdown");
	let api = new PlannedActivityResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.plannedActivityResourceGetPlannedActivityResourceForDropdown());
	result?.warning && rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetPlannedActivityResourceForDropdown");
	return result;
}
