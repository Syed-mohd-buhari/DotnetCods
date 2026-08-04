import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { PlannedActivityTypesApi } from "../../../Business/PlannedActivityTypesBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_PLANNED_ACTIVITY_TYPES, RESTORE_PLANNED_ACTIVITY_TYPES } from "../../../Model/PlannedActivityTypes";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deletePlannedActivityTypes(id: number) {
	setLoader("ADD", "deletePlannedActivityTypes");
	let api = new PlannedActivityTypesApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.PlannedActivityTypesDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_PLANNED_ACTIVITY_TYPES, payload: rtn });
	setLoader("REMOVE", "deletePlannedActivityTypes");
	return rtn;
}
