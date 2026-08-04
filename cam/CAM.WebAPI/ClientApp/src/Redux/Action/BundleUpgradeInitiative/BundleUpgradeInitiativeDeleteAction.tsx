import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { BundleUpgradeInitiativeApi } from "../../../Business/BundleUpgradeInitiativeBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_BUNDLE_UPGRADE_INIZIATIVE, RESTORE_BUNDLE_UPGRADE_INIZIATIVE } from "../../../Model/BundleUpgradeIniziative";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteBundleUpgradeInitiative(id: number) {
	setLoader("ADD", "deleteBundleUpgradeInitiative");
	let api = new BundleUpgradeInitiativeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.bundleUpgradeInitiativeDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_BUNDLE_UPGRADE_INIZIATIVE, payload: rtn });
	setLoader("REMOVE", "deleteBundleUpgradeInitiative");
	return rtn;
}

export async function DeleteDeepBundleUpgradeInitiative(id: number) {
	let api = new BundleUpgradeInitiativeApi();
	setLoader("ADD", "DeleteDeepBundleUpgradeInitiative");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.bundleUpgradeInitiativeDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: DELETE_BUNDLE_UPGRADE_INIZIATIVE, payload: rtn });
	setLoader("REMOVE", "DeleteDeepBundleUpgradeInitiative");
	return rtn;
}

export async function GetRelatedRecordsBundleUpgradeInitiative(id: number) {
	let api = new BundleUpgradeInitiativeApi();
	setLoader("ADD", "GetRelatedRecordsBundleUpgradeInitiative");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.bundleUpgradeInitiativeGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	setLoader("REMOVE", "GetRelatedRecordsBundleUpgradeInitiative");
	return rtn;
}

export async function RestoreBundleUpgradeInitiative(id: number) {
	setLoader("ADD", "RestoreBundleUpgradeInitiative");
	let api = new BundleUpgradeInitiativeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.bundleUpgradeInitiativeRestore(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: RESTORE_BUNDLE_UPGRADE_INIZIATIVE, payload: rtn });
	setLoader("REMOVE", "RestoreBundleUpgradeInitiative");
	return rtn;
}
