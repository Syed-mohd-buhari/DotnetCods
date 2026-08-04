import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { BundleUpgradeInitiativeApiFetchParamCreator, BundleUpgradeInitiativeApi } from "../../../Business/BundleUpgradeInitiativeBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { BundleUpgradeInitiativeDto, BundleUpgradeInitiativeDtoUpdate, BundleUpgradeInitiativeEdit, EDIT_BUNDLE_UPGRADE_INIZIATIVE, GET_EDIT_BUNDLE_UPGRADE_INIZIATIVE } from "../../../Model/BundleUpgradeIniziative";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetBundleUpgradeInitiativeEditResource(id: number) {
	setLoader("ADD", "GetBundleUpgradeInitiativeEditResource");
	let api = new BundleUpgradeInitiativeApi();
	let createResource = await ApiCallWithErrorHandling<Promise<BundleUpgradeInitiativeDto>>(() => api.bundleUpgradeInitiativeGetUpdateResourceBundleUpgradeInitiative(id));
	let rtn = { BundleUpgradeInitiativeDtoEdit: createResource } as BundleUpgradeInitiativeEdit;
	rootStore.dispatch({ type: GET_EDIT_BUNDLE_UPGRADE_INIZIATIVE, payload: rtn });
	setLoader("REMOVE", "GetBundleUpgradeInitiativeEditResource");

	return rtn;
}

export async function EditBundleUpgradeInitiative(data: BundleUpgradeInitiativeDtoUpdate, forced?: boolean) {
	setLoader("ADD", "EditBundleUpgradeInitiative");
	let api = new BundleUpgradeInitiativeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.bundleUpgradeInitiativePut(data, forced));
	let rtn = { ResultDtoEdit: result } as BundleUpgradeInitiativeEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: EDIT_BUNDLE_UPGRADE_INIZIATIVE, payload: rtn });
	setLoader("REMOVE", "EditBundleUpgradeInitiative");
	return rtn;
}
