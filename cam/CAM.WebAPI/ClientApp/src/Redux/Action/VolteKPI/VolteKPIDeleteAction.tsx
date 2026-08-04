import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { VolteKPIApi } from "../../../Business/VolteKPIBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_VOLTE_KPI, RESTORE_VOLTE_KPI } from "../../../Model/VolteKpi/VolteKPI";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteVolteKPI(id: number) {
	setLoader("ADD", "deleteVolteKPI");
	let api = new VolteKPIApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.volteKPIDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_VOLTE_KPI, payload: rtn });
	setLoader("REMOVE", "deleteVolteKPI");
	return rtn;
}
export async function RestoreVolteKPI(id: number) {
	setLoader("ADD", "RestoreVolteKPI");
	let api = new VolteKPIApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.volteKPIRestore(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: RESTORE_VOLTE_KPI, payload: rtn });
	setLoader("REMOVE", "RestoreVolteKPI");
	return rtn;
}
