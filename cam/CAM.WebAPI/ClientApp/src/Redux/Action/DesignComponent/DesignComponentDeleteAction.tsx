import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignComponentApi } from "../../../Business/DesignComponentBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_DESIGN_COMPONENT, RESTORE_DESIGN_COMPONENT } from "../../../Model/DesignComponent";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteDesignComponent(id: number) {
	setLoader("ADD", "deleteDesignComponent");
	let api = new DesignComponentApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.designComponentDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_DESIGN_COMPONENT, payload: rtn });
	setLoader("REMOVE", "deleteDesignComponent");
	return rtn;
}

export async function DeleteDeepDesignComponent(id: number) {
	let api = new DesignComponentApi();
	setLoader("ADD", "DeleteDeepDesignComponent");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.designComponentDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_DESIGN_COMPONENT, payload: rtn });
	setLoader("REMOVE", "DeleteDeepDesignComponent");
	return rtn;
}

export async function GetRelatedRecordsDesignComponent(id: number) {
	let api = new DesignComponentApi();
	setLoader("ADD", "GetRelatedRecordsDesignComponent");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.designComponentGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsDesignComponent");
	return rtn;
}

export async function RestoreDesignComponent(id: number) {
	setLoader("ADD", "RestoreDesignComponent");
	let api = new DesignComponentApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.designComponentRestore(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: RESTORE_DESIGN_COMPONENT, payload: rtn });
	setLoader("REMOVE", "RestoreDesignComponent");
	return rtn;
}
