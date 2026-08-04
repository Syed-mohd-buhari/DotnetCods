import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { OpCoApi } from "../../../../Business/LookUp/OpCoBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetOpCoEditResource(id: number) {
	setLoader("ADD", "GetOpCoEditResource");

	let api = new OpCoApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.opCoGetUpdateResourceOpCo(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_OP_CO", payload: rtn });
	setLoader("REMOVE", "GetOpCoEditResource");

	return rtn;
}

export async function EditOpCo(data: TipologicaGridDto) {
	setLoader("ADD", "EditOpCo");
	let api = new OpCoApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.opCoPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_OP_CO", payload: rtn });
	setLoader("REMOVE", "EditOpCo");
	return rtn;
}
