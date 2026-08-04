import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { HardwareTypeApi } from "../../../../Business/LookUp/HardwareTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetHardwareTypeCreateResource() {
	setLoader("ADD", "GetHardwareTypeCreateResource");

	let api = new HardwareTypeApi();

	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.hardwareTypeGetCreateResourceHardwareType());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_HARDWARE_TYPE", payload: rtn });
	setLoader("REMOVE", "GetHardwareTypeCreateResource");
}

export async function CreatHardwareType(data: TipologicaGridDto) {
	setLoader("ADD", "CreatHardwareType");
	let api = new HardwareTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.hardwareTypeCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_HARDWARE_TYPE", payload: rtn });
	setLoader("REMOVE", "CreatHardwareType");
	return rtn;
}
