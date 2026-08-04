import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlannedActivityNetworkElementApi } from "../../../../Business/LookUp/PlannedActivityNetworkElementBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import { PlannedActivityNetworkElementDtoUpdate } from "../../../../Model/LookUp/PlannedActivityNetworkElement";

export async function GetPlannedActivityNetworkElementEditResource(id: number) {
	setLoader("ADD", "GetPlannedActivityNetworkElementEditResource");

	let api = new PlannedActivityNetworkElementApi();
	let createResource = await ApiCallWithErrorHandling<Promise<PlannedActivityNetworkElementDtoUpdate>>(() => api.plannedActivityNetworkElementGetUpdateResourcePlannedActivityNetworkElement(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_PLANNED_ACTIVITY_NETWORK_ELEMENT", payload: rtn });
	setLoader("REMOVE", "GetPlannedActivityNetworkElementEditResource");

	return rtn;
}

export async function EditPlannedActivityNetworkElement(data: TipologicaGridDto) {
	setLoader("ADD", "EditPlannedActivityNetworkElement");
	let api = new PlannedActivityNetworkElementApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.plannedActivityNetworkElementPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_PLANNED_ACTIVITY_NETWORK_ELEMENT", payload: rtn });
	setLoader("REMOVE", "EditPlannedActivityNetworkElement");
	return rtn;
}
