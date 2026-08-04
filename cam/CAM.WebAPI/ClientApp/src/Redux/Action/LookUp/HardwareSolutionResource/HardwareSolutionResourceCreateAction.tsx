import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { HardwareSolutionResourceApi } from "../../../../Business/LookUp/HardwareSolutionResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetHardwareSolutionResourceCreateResource() {
	setLoader("ADD", "GetHardwareSolutionResourceCreateResource");

	let api = new HardwareSolutionResourceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.hardwareSolutionResourceGetCreateResourceHardwareSolutionResource());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_HARDWARE_SOLUTION_RESOURCE", payload: rtn });
	setLoader("REMOVE", "GetHardwareSolutionResourceCreateResource");
}

export async function CreatHardwareSolutionResource(data: TipologicaGridDto) {
	setLoader("ADD", "CreatHardwareSolutionResource");
	let api = new HardwareSolutionResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.hardwareSolutionResourceCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_HARDWARE_SOLUTION_RESOURCE", payload: rtn });
	setLoader("REMOVE", "CreatHardwareSolutionResource");
	return rtn;
}
