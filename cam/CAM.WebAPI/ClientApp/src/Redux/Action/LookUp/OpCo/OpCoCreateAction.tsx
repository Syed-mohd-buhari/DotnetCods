import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { OpCoApi } from "../../../../Business/LookUp/OpCoBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetOpCoCreateResource() {
	setLoader("ADD", "GetOpCoCreateResource");

	let api = new OpCoApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.opCoGetCreateResourceOpCo());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_OP_CO", payload: rtn });
	setLoader("REMOVE", "GetOpCoCreateResource");
}

export async function CreatOpCo(data: TipologicaGridDto) {
	setLoader("ADD", "CreatOpCo");
	let api = new OpCoApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.opCoCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_OP_CO", payload: rtn });
	setLoader("REMOVE", "CreatOpCo");
	return rtn;
}
