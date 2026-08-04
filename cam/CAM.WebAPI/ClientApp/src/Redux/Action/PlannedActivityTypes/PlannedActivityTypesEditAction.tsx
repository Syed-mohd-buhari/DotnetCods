import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { PlannedActivityTypesApiFetchParamCreator, PlannedActivityTypesApi } from "../../../Business/PlannedActivityTypesBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { PlannedActivityTypesDtoUpdate, PlannedActivityTypesEdit, EDIT_PLANNED_ACTIVITY_TYPES, GET_EDIT_PLANNED_ACTIVITY_TYPES } from "../../../Model/PlannedActivityTypes";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function EditPlannedActivityTypes(data: PlannedActivityTypesDtoUpdate, forced?: boolean) {
	setLoader("ADD", "EditPlannedActivityTypes");
	let api = new PlannedActivityTypesApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.PlannedActivityTypesUpdate(data, forced));
	let rtn = { ResultDtoEdit: result } as PlannedActivityTypesEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: EDIT_PLANNED_ACTIVITY_TYPES, payload: rtn });
	setLoader("REMOVE", "EditPlannedActivityTypes");
	return rtn;
}
