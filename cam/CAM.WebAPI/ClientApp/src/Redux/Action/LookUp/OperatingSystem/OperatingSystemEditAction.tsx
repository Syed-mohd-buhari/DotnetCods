import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { OperatingSystemApi } from "../../../../Business/LookUp/OperatingSystemBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetOperatingSystemEditResource(id: number) {
	setLoader("ADD", "GetOperatingSystemEditResource");

	let api = new OperatingSystemApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.operatingSystemGetUpdateResourceOperatingSystem(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_OPERATING_SYSTEM", payload: rtn });
	setLoader("REMOVE", "GetOperatingSystemEditResource");

	return rtn;
}

export async function EditOperatingSystem(data: TipologicaGridDto) {
	setLoader("ADD", "EditOperatingSystem");
	let api = new OperatingSystemApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.operatingSystemPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_OPERATING_SYSTEM", payload: rtn });
	setLoader("REMOVE", "EditOperatingSystem");
	return rtn;
}
