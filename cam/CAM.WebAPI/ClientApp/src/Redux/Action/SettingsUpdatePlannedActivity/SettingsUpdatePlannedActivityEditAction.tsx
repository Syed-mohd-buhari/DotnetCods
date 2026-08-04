import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { SettingsUpdatePlannedActivityApiFetchParamCreator, SettingsUpdatePlannedActivityApi } from "../../../Business/SettingsUpdatePlannedActivityBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { SettingsUpdatePlannedActivityDtoUpdate, SettingsUpdatePlannedActivityEdit, EDIT_SETTINGS_UPDATE_PLANNED_ACTIVITY, GET_EDIT_SETTINGS_UPDATE_PLANNED_ACTIVITY } from "../../../Model/SettingsUpdatePlannedActivity";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSettingsUpdatePlannedActivityEditResource(id: number) {
	setLoader("ADD", "GetSettingsUpdatePlannedActivityEditResource");

	// const dispach = useDispatch();
	let api = new SettingsUpdatePlannedActivityApi();
	let createResource = await ApiCallWithErrorHandling<Promise<SettingsUpdatePlannedActivityDtoUpdate>>(() => api.settingsUpdatePlannedActivityGetUpdateResourceSettingsUpdatePlannedActivity(id));
	let rtn = { SettingsUpdatePlannedActivityDtoEdit: createResource } as SettingsUpdatePlannedActivityEdit;
	rootStore.dispatch({ type: GET_EDIT_SETTINGS_UPDATE_PLANNED_ACTIVITY, payload: rtn });
	setLoader("REMOVE", "GetSettingsUpdatePlannedActivityEditResource");

	return rtn;
}

export async function EditSettingsUpdatePlannedActivity(data: SettingsUpdatePlannedActivityDtoUpdate, forced?: boolean) {
	setLoader("ADD", "EditSettingsUpdatePlannedActivity");
	let api = new SettingsUpdatePlannedActivityApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.settingsUpdatePlannedActivityPut(data, forced));
	let rtn = { ResultDtoEdit: result } as SettingsUpdatePlannedActivityEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: EDIT_SETTINGS_UPDATE_PLANNED_ACTIVITY, payload: rtn });
	setLoader("REMOVE", "EditSettingsUpdatePlannedActivity");
	return rtn;
}
