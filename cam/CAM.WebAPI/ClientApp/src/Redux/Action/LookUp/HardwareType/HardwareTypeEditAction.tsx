import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { HardwareTypeApi } from "../../../../Business/LookUp/HardwareTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetHardwareTypeEditResource(id: number) {
	setLoader("ADD", "GetHardwareTypeEditResource");

	// const dispach = useDispatch();
	let api = new HardwareTypeApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.hardwareTypeGetUpdateResourceHardwareType(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_HARDWARE_TYPE", payload: rtn });
	setLoader("REMOVE", "GetHardwareTypeEditResource");

	return rtn;
}

export async function EditHardwareType(data: TipologicaGridDto) {
	let api = new HardwareTypeApi();
	setLoader("ADD", "EditHardwareType");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.hardwareTypePut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_HARDWARE_TYPE", payload: rtn });
	setLoader("REMOVE", "EditHardwareType");
	return rtn;
}
