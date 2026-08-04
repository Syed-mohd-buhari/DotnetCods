import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { FullOrPartialResourceApi } from "../../../../Business/LookUp/FullOrPartialResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetFullOrPartialResourceEditResource(id: number) {
	setLoader("ADD", "GetFullOrPartialResourceEditResource");

	let api = new FullOrPartialResourceApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.fullOrPartialResourceGetUpdateResourceFullOrPartialResource(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_F_O_P_R", payload: rtn });
	setLoader("REMOVE", "GetFullOrPartialResourceEditResource");

	return rtn;
}

export async function EditFullOrPartialResource(data: TipologicaGridDto) {
	setLoader("ADD", "EditFullOrPartialResource");
	let api = new FullOrPartialResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.fullOrPartialResourcePut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_F_O_P_R", payload: rtn });
	setLoader("REMOVE", "EditFullOrPartialResource");
	return rtn;
}
