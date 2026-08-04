import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ActivityDetailsApi } from "../../../../Business/LookUp/ActivityDetailsBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetActivityDetailsCreateResource() {
	setLoader("ADD", "GetActivityDetailsCreateResource");
	let api = new ActivityDetailsApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.activityDetailsGetCreateResourceActivityDetails());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_ACTIVITY_DETAILS", payload: rtn });
	setLoader("REMOVE", "GetActivityDetailsCreateResource");
}

export async function CreatActivityDetails(data: TipologicaGridDto) {
	setLoader("ADD", "CreatActivityDetails");
	let api = new ActivityDetailsApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.activityDetailsCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_ACTIVITY_DETAILS", payload: rtn });
	setLoader("REMOVE", "CreatActivityDetails");
	return rtn;
}
