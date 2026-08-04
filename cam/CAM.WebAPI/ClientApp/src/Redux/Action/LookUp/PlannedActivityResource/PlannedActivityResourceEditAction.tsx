import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlannedActivityResourceApi } from "../../../../Business/LookUp/PlannedActivityResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetPlannedActivityResourceEditResource(id: number) {
	setLoader("ADD", "GetPlannedActivityResourceEditResource");

	let api = new PlannedActivityResourceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.plannedActivityResourceGetUpdateResourcePlannedActivityResource(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_PLANNED_ACTIVITY_RESOURCE", payload: rtn });
	setLoader("REMOVE", "GetPlannedActivityResourceEditResource");

	return rtn;
}

export async function EditPlannedActivityResource(data: TipologicaGridDto) {
	setLoader("ADD", "EditPlannedActivityResource");
	let api = new PlannedActivityResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.plannedActivityResourcePut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_PLANNED_ACTIVITY_RESOURCE", payload: rtn });
	setLoader("REMOVE", "EditPlannedActivityResource");
	return rtn;
}
