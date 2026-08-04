import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { LCMPlannedActionResourceApi } from "../../../../Business/LookUp/LCMPlannedActionResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetLCMPlannedActionResourceEditResource(id: number) {
	setLoader("ADD", "GetLCMPlannedActionResourceEditResource");

	let api = new LCMPlannedActionResourceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.lCMPlannedActionResourceGetUpdateResourceLCMPlannedActionResource(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_LCM_PLANNED_ACTION_RESOURCE", payload: rtn });
	setLoader("REMOVE", "GetLCMPlannedActionResourceEditResource");

	return rtn;
}

export async function EditLCMPlannedActionResource(data: TipologicaGridDto) {
	setLoader("ADD", "EditLCMPlannedActionResource");
	let api = new LCMPlannedActionResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.lCMPlannedActionResourcePut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_LCM_PLANNED_ACTION_RESOURCE", payload: rtn });
	setLoader("REMOVE", "EditLCMPlannedActionResource");
	return rtn;
}
