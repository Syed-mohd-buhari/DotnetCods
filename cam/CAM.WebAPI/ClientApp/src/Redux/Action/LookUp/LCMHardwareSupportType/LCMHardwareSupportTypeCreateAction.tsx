import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { LCMHardwareSupportTypeApi } from "../../../../Business/LookUp/LCMHardwareSupportTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetLCMHardwareSupportTypeCreateResource() {
	setLoader("ADD", "GetLCMHardwareSupportTypeCreateResource");

	let api = new LCMHardwareSupportTypeApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.lCMHardwareSupportTypeGetCreateResourceLCMHardwareSupportType());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_LCM_HW_SUPPORT_TYPE", payload: rtn });
	setLoader("REMOVE", "GetLCMHardwareSupportTypeCreateResource");
}

export async function CreatLCMHardwareSupportType(data: TipologicaGridDto) {
	let api = new LCMHardwareSupportTypeApi();
	setLoader("ADD", "CreatLCMHardwareSupportType");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.lCMHardwareSupportTypeCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_LCM_HW_SUPPORT_TYPE", payload: rtn });
	setLoader("REMOVE", "CreatLCMHardwareSupportType");
	return rtn;
}
