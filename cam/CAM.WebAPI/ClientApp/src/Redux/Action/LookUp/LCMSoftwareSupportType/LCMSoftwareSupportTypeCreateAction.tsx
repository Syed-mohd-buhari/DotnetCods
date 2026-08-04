import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { LCMSoftwareSupportTypeApi } from "../../../../Business/LookUp/LCMSoftwareSupportTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LCMSoftwareSupportTypOperational, LCMSoftwareSupportTypeDto } from "../../../../Model/LookUp/LCMSoftwareSupportType";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetLCMSoftwareSupportTypeCreateResource() {
	setLoader("ADD", "GetLCMSoftwareSupportTypeCreateResource");

	let api = new LCMSoftwareSupportTypeApi();

	let createResource = await ApiCallWithErrorHandling<Promise<LCMSoftwareSupportTypeDto>>(() => api.lCMSoftwareSupportTypeGetCreateResourceLCMSoftwareSupportType());
	let rtn = { ResultDtoCreate: null, Create: createResource } as LCMSoftwareSupportTypOperational;
	rootStore.dispatch({ type: "GET_CREATE_LCM_SOFTWARE_SUPPORT_TYPE", payload: rtn });
	setLoader("REMOVE", "GetLCMSoftwareSupportTypeCreateResource");
}

export async function CreatLCMSoftwareSupportType(data: LCMSoftwareSupportTypeDto) {
	setLoader("ADD", "CreatLCMSoftwareSupportType");
	let api = new LCMSoftwareSupportTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.lCMSoftwareSupportTypeCreate(data));
	let rtn = { ResultDtoCreate: result, Create: null } as LCMSoftwareSupportTypOperational;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_LCM_SOFTWARE_SUPPORT_TYPE", payload: rtn });
	setLoader("REMOVE", "CreatLCMSoftwareSupportType");
	return rtn;
}
