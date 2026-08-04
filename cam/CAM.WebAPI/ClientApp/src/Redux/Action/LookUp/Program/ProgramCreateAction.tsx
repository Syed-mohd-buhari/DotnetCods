import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DriverApi } from "../../../../Business/LookUp/ProgramBuisness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetProgramCreateResource() {
	setLoader("ADD", "GetProgramCreateResource");

	let api = new DriverApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.programGetCreateResourceProgram());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_PROGRAM", payload: rtn });
	setLoader("REMOVE", "GetProgramCreateResource");
}

export async function CreatProgram(data: TipologicaGridDto) {
	setLoader("ADD", "CreatProgram");
	let api = new DriverApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.programCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_PROGRAM", payload: rtn });
	setLoader("REMOVE", "CreatProgram");
	return rtn;
}
