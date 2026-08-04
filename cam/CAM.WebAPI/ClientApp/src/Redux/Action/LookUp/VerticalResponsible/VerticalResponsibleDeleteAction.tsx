import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VerticalResponsibleApi } from "../../../../Business/LookUp/VerticalResponsibleBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function deleteVerticalResponsible(id: number) {
	setLoader("ADD", "deleteVerticalResponsible");
	let api = new VerticalResponsibleApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.verticalResponsibleDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_VERTICAL_RESPONSIBLE", payload: rtn });
	setLoader("REMOVE", "deleteVerticalResponsible");
	return rtn;
}

export async function DeleteDeepVerticalResponsible(id: number) {
	let api = new VerticalResponsibleApi();
	setLoader("ADD", "DeleteDeepVerticalResponsible");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.verticalResponsibleDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_VERTICAL_RESPONSIBLE", payload: rtn });
	setLoader("REMOVE", "DeleteDeepVerticalResponsible");
	return rtn;
}

export async function GetRelatedRecordsVerticalResponsible(id: number) {
	let api = new VerticalResponsibleApi();
	setLoader("ADD", "GetRelatedRecordsVerticalResponsible");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.verticalResponsibleGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsVerticalResponsible");
	return rtn;
}
