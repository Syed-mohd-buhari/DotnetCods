import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ActivityStatusApi } from "../../../../Business/LookUp/ActivityStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, LookUpCreateRule, TipologicaGridDto, TipologicaGridDtoCombinationRule } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetActivityStatusCreateResource() {
	setLoader("ADD", "GetActivityStatusCreateResource");

	let api = new ActivityStatusApi();

	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDtoCombinationRule>>(() => api.activityStatusGetCreateResourceActivityStatus());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreateRule;
	rootStore.dispatch({ type: "GET_CREATE_ACTIVITY_STATUS", payload: rtn });
	setLoader("REMOVE", "GetActivityStatusCreateResource");
}

export async function CreatActivityStatus(data: TipologicaGridDtoCombinationRule) {
	setLoader("ADD", "CreatActivityStatus");
	let api = new ActivityStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.activityStatusCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreateRule;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_ACTIVITY_STATUS", payload: rtn });
	setLoader("REMOVE", "CreatActivityStatus");
	return rtn;
}
