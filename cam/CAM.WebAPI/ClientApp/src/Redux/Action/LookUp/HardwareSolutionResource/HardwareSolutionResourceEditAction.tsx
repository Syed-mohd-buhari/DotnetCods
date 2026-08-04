import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { HardwareSolutionResourceApi } from "../../../../Business/LookUp/HardwareSolutionResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetHardwareSolutionResourceEditResource(id: number) {
	setLoader("ADD", "GetHardwareSolutionResourceEditResource");

	// const dispach = useDispatch();
	let api = new HardwareSolutionResourceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.hardwareSolutionResourceGetUpdateResourceHardwareSolutionResource(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_HARDWARE_SOLUTION_RESOURCE", payload: rtn });
	setLoader("REMOVE", "GetHardwareSolutionResourceEditResource");

	return rtn;
}

export async function EditHardwareSolutionResource(data: TipologicaGridDto) {
	setLoader("ADD", "EditHardwareSolutionResource");
	let api = new HardwareSolutionResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.hardwareSolutionResourcePut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_HARDWARE_SOLUTION_RESOURCE", payload: rtn });
	setLoader("REMOVE", "EditHardwareSolutionResource");
	return rtn;
}
