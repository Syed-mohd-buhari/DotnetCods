import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlannedActivityNetworkElementApi } from "../../../../Business/LookUp/PlannedActivityNetworkElementBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deletePlannedActivityNetworkElement(id: number) {
	setLoader("ADD", "deletePlannedActivityNetworkElement");
	let api = new PlannedActivityNetworkElementApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.plannedActivityNetworkElementDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_PLANNED_ACTIVITY_NETWORK_ELEMENT", payload: rtn });
	setLoader("REMOVE", "deletePlannedActivityNetworkElement");
	return rtn;
}
