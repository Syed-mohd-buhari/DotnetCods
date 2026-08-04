import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlatformApi } from "../../../../Business/LookUp/PlatformBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetPlatformEditResource(id: number) {
	setLoader("ADD", "GetPlatformEditResource");

	// const dispach = useDispatch();
	let api = new PlatformApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.platformGetUpdateResourcePlatform(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_PLATFORM", payload: rtn });
	setLoader("REMOVE", "GetPlatformEditResource");

	return rtn;
}

export async function EditPlatform(data: TipologicaGridDto) {
	let api = new PlatformApi();
	setLoader("ADD", "EditPlatform");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.platformPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_PLATFORM", payload: rtn });
	setLoader("REMOVE", "EditPlatform");
	return rtn;
}
