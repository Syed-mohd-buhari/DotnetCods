import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { EnvironmentApi } from "../../../../Business/LookUp/EnvironmentBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetEnvironmentEditResource(id: number) {
	setLoader("ADD", "GetEnvironmentEditResource");

	let api = new EnvironmentApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.environmentGetUpdateResourceEnvironment(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_ENVIRONMENT", payload: rtn });
	setLoader("REMOVE", "GetEnvironmentEditResource");

	return rtn;
}

export async function EditEnvironment(data: TipologicaGridDto) {
	setLoader("ADD", "EditEnvironment");
	let api = new EnvironmentApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.environmentPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_ENVIRONMENT", payload: rtn });
	setLoader("REMOVE", "EditEnvironment");
	return rtn;
}
