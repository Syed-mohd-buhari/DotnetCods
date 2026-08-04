import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlannedActivityNetworkElementApi } from "../../../../Business/LookUp/PlannedActivityNetworkElementBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetPlannedActivityNetworkElementCreateResource() {
	setLoader("ADD", "GetPlannedActivityNetworkElementCreateResource");

	let api = new PlannedActivityNetworkElementApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.plannedActivityNetworkElementGetCreateResourcePlannedActivityNetworkElement());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_PLANNED_ACTIVITY_NETWORK_ELEMENT", payload: rtn });
	setLoader("REMOVE", "GetPlannedActivityNetworkElementCreateResource");
}

export async function CreatPlannedActivityNetworkElement(data: TipologicaGridDto) {
	setLoader("ADD", "CreatPlannedActivityNetworkElement");
	let api = new PlannedActivityNetworkElementApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.plannedActivityNetworkElementCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_PLANNED_ACTIVITY_NETWORK_ELEMENT", payload: rtn });
	setLoader("REMOVE", "CreatPlannedActivityNetworkElement");
	return rtn;
}
