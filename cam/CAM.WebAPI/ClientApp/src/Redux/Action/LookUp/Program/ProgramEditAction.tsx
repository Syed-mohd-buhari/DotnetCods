import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DriverApi } from "../../../../Business/LookUp/ProgramBuisness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetProgramEditResource(id: number) {
	setLoader("ADD", "GetProgramEditResource");

	let api = new DriverApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.programGetUpdateResourceProgram(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_PROGRAM", payload: rtn });
	setLoader("REMOVE", "GetProgramEditResource");

	return rtn;
}

export async function EditProgram(data: TipologicaGridDto) {
	setLoader("ADD", "EditProgram");
	let api = new DriverApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.programPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_PROGRAM", payload: rtn });
	setLoader("REMOVE", "EditProgram");
	return rtn;
}
