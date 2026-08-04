import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { FullOrPartialResourceApi } from "../../../../Business/LookUp/FullOrPartialResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetFullOrPartialResourceCreateResource() {
	setLoader("ADD", "GetFullOrPartialResourceCreateResource");

	let api = new FullOrPartialResourceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.fullOrPartialResourceGetCreateResourceFullOrPartialResource());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_F_O_P_R", payload: rtn });
	setLoader("REMOVE", "GetFullOrPartialResourceCreateResource");
}

export async function CreatFullOrPartialResource(data: TipologicaGridDto) {
	setLoader("ADD", "CreatFullOrPartialResource");
	let api = new FullOrPartialResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.fullOrPartialResourceCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_F_O_P_R", payload: rtn });
	setLoader("REMOVE", "CreatFullOrPartialResource");
	return rtn;
}
