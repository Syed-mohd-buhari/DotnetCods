import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { SettingsUpdatePlannedActivityApi } from "../../../Business/SettingsUpdatePlannedActivityBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_SETTINGS_UPDATE_PLANNED_ACTIVITY, RESTORE_SETTINGS_UPDATE_PLANNED_ACTIVITY } from "../../../Model/SettingsUpdatePlannedActivity";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteSettingsUpdatePlannedActivity(id: number) {
	setLoader("ADD", "deleteSettingsUpdatePlannedActivity");
	let api = new SettingsUpdatePlannedActivityApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.settingsUpdatePlannedActivityDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_SETTINGS_UPDATE_PLANNED_ACTIVITY, payload: rtn });
	setLoader("REMOVE", "deleteSettingsUpdatePlannedActivity");
	return rtn;
}

export async function DeleteDeepSettingsUpdatePlannedActivity(id: number) {
	let api = new SettingsUpdatePlannedActivityApi();
	setLoader("ADD", "DeleteDeepSettingsUpdatePlannedActivity");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.settingsUpdatePlannedActivityDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_SETTINGS_UPDATE_PLANNED_ACTIVITY, payload: rtn });
	setLoader("REMOVE", "DeleteDeepSettingsUpdatePlannedActivity");
	return rtn;
}

export async function GetRelatedRecordsSettingsUpdatePlannedActivity(id: number) {
	let api = new SettingsUpdatePlannedActivityApi();
	setLoader("ADD", "GetRelatedRecordsSettingsUpdatePlannedActivity");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.settingsUpdatePlannedActivityGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsSettingsUpdatePlannedActivity");
	return rtn;
}

// export async function RestoreSettingsUpdatePlannedActivity(id: number) {
//      ;
//     let api = new SettingsUpdatePlannedActivityApi();
//     let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(()=>api.settingsUpdatePlannedActivityRestore(id));
//     let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
//     rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
//     rootStore.dispatch({type:RESTORE_SETTINGS_UPDATE_PLANNED_ACTIVITY,payload:rtn});
//      ;
//      return rtn;
// }
