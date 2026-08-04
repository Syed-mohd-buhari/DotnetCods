import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { LCMPlannedActionResourceApi } from "../../../../Business/LookUp/LCMPlannedActionResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetLCMPlannedActionResourceCreateResource() {
	setLoader("ADD", "GetLCMPlannedActionResourceCreateResource");

	let api = new LCMPlannedActionResourceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.lCMPlannedActionResourceGetCreateResourceLCMPlannedActionResource());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_LCM_PLANNED_ACTION_RESOURCE", payload: rtn });
	setLoader("REMOVE", "GetLCMPlannedActionResourceCreateResource");
}

export async function CreatLCMPlannedActionResource(data: TipologicaGridDto) {
	setLoader("ADD", "CreatLCMPlannedActionResource");
	let api = new LCMPlannedActionResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.lCMPlannedActionResourceCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_LCM_PLANNED_ACTION_RESOURCE", payload: rtn });
	setLoader("REMOVE", "CreatLCMPlannedActionResource");
	return rtn;
}
