import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { LCMSoftwareSupportTypeApi } from "../../../../Business/LookUp/LCMSoftwareSupportTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LCMSoftwareSupportTypOperational, LCMSoftwareSupportTypeDto, LCMSoftwareSupportTypOperationalEdit } from "../../../../Model/LookUp/LCMSoftwareSupportType";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetLCMSoftwareSupportTypeEditResource(id: number) {
	setLoader("ADD", "GetLCMSoftwareSupportTypeEditResource");

	let api = new LCMSoftwareSupportTypeApi();
	let createResource = await ApiCallWithErrorHandling<Promise<LCMSoftwareSupportTypeDto>>(() => api.lCMSoftwareSupportTypeGetUpdateResourceLCMSoftwareSupportType(id));
	let rtn = { Create: createResource } as LCMSoftwareSupportTypOperationalEdit;
	rootStore.dispatch({ type: "GET_EDIT_LCM_SOFTWARE_SUPPORT_TYPE", payload: rtn });
	setLoader("REMOVE", "GetLCMSoftwareSupportTypeEditResource");

	return rtn;
}

export async function EditLCMSoftwareSupportType(data: LCMSoftwareSupportTypeDto) {
	setLoader("ADD", "EditLCMSoftwareSupportType");
	let api = new LCMSoftwareSupportTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.lCMSoftwareSupportTypePut(data));
	let rtn = { ResultDtoEdit: result } as LCMSoftwareSupportTypOperationalEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_LCM_SOFTWARE_SUPPORT_TYPE", payload: rtn });
	setLoader("REMOVE", "EditLCMSoftwareSupportType");
	return rtn;
}
