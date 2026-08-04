import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { RelatesToResourceApi } from "../../../../Business/LookUp/RelatesToResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetRelatesToResourceEditResource(id: number) {
	setLoader("ADD", "GetRelatesToResourceEditResource");

	let api = new RelatesToResourceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.relatesToResourceGetUpdateResourceRelatesToResource(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_RELATES_TO_RESOURCE", payload: rtn });
	setLoader("REMOVE", "GetRelatesToResourceEditResource");

	return rtn;
}

export async function EditRelatesToResource(data: TipologicaGridDto) {
	let api = new RelatesToResourceApi();
	setLoader("ADD", "EditRelatesToResource");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.relatesToResourcePut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_RELATES_TO_RESOURCE", payload: rtn });
	setLoader("REMOVE", "EditRelatesToResource");
	return rtn;
}
