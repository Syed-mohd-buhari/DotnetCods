import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { LCMHardwareSupportTypeApi } from "../../../../Business/LookUp/LCMHardwareSupportTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetLCMHardwareSupportTypeEditResource(id: number) {
	setLoader("ADD", "GetLCMHardwareSupportTypeEditResource");

	let api = new LCMHardwareSupportTypeApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.lCMHardwareSupportTypeGetUpdateResourceLCMHardwareSupportType(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_LCM_HW_SUPPORT_TYPE", payload: rtn });
	setLoader("REMOVE", "GetLCMHardwareSupportTypeEditResource");

	return rtn;
}

export async function EditLCMHardwareSupportType(data: TipologicaGridDto) {
	setLoader("ADD", "EditLCMHardwareSupportType");
	let api = new LCMHardwareSupportTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.lCMHardwareSupportTypePut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_LCM_HW_SUPPORT_TYPE", payload: rtn });
	setLoader("REMOVE", "EditLCMHardwareSupportType");
	return rtn;
}
