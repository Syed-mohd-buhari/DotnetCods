import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ActivityStatusApi } from "../../../../Business/LookUp/ActivityStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, LookUpEditRule, TipologicaGridDtoCombinationRule } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetActivityStatusEditResource(id: number) {
	setLoader("ADD", "GetActivityStatusEditResource");

	// const dispach = useDispatch();
	let api = new ActivityStatusApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDtoCombinationRule>>(() => api.activityStatusGetUpdateResourceActivityStatus(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEditRule;
	rootStore.dispatch({ type: "GET_EDIT_ACTIVITY_STATUS", payload: rtn });
	setLoader("REMOVE", "GetActivityStatusEditResource");

	return rtn;
}

export async function EditActivityStatus(data: TipologicaGridDtoCombinationRule) {
	let api = new ActivityStatusApi();
	setLoader("ADD", "EditActivityStatus");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.activityStatusPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDtoCombinationRule;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_ACTIVITY_STATUS", payload: rtn });
	setLoader("REMOVE", "EditActivityStatus");
	return rtn;
}
