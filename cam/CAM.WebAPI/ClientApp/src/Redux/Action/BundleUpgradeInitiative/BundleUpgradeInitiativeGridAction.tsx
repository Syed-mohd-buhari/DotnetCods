// eslint-disable-next-line @typescript-eslint/no-unused-vars
import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../Business/Common/CommonBusiness";
import { BundleUpgradeInitiativeApi } from "../../../Business/BundleUpgradeInitiativeBusiness";
import {
	BundleUpgradeInitiativeGrid,
	BundleUpgradeInitiativeQueryObjectGrid,
	GET_FILTER_BUNDLE_UPGRADE_INIZIATIVE,
	GET_GRID_BUNDLE_UPGRADE_INIZIATIVE,
	QueryResultDtoOfBundleUpgradeInitiativeDtoGrid,
} from "../../../Model/BundleUpgradeIniziative";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import { setNotification } from "../NotificationAction";
import setLoader from "../LoaderAction";
// import { useDispatch } from 'react-redux'

export async function GetBundleUpgradeInitiativeGrid(queryFilter?: BundleUpgradeInitiativeQueryObjectGrid) {
	let result: QueryResultDtoOfBundleUpgradeInitiativeDtoGrid | null | undefined;
	let api = new BundleUpgradeInitiativeApi();
	setLoader("ADD", "GetBundleUpgradeInitiativeGrid");

	try {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfBundleUpgradeInitiativeDtoGrid>>(() =>
				api.bundleUpgradeInitiativeGetBundleUpgradeInitiative(
					queryFilter || {}
				)
			);
		
		let rtn = { BundleUpgradeInitiativeGridResult: result, filter: null } as BundleUpgradeInitiativeGrid;
		rootStore.dispatch({ type: GET_GRID_BUNDLE_UPGRADE_INIZIATIVE, payload: rtn as BundleUpgradeInitiativeGrid });
	} catch (error) {
		rootStore.dispatch({ type: GET_GRID_BUNDLE_UPGRADE_INIZIATIVE, payload: { BundleUpgradeInitiativeGridResult: result, filter: null } as BundleUpgradeInitiativeGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetBundleUpgradeInitiativeGrid");
}

export async function GetFilterColumBundleUpgradeInitiative(columName: string, columValue: string, queryFilter?: BundleUpgradeInitiativeQueryObjectGrid) {
	let result: FilterValueDto[] | undefined;
	let api = new BundleUpgradeInitiativeApi();

		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.bundleUpgradeInitiativeGetFilterResult(
				queryFilter || {},
				columName,
				columValue
			)
		);
	
	let rtn = { filter: result, BundleUpgradeInitiativeGridResult: null } as BundleUpgradeInitiativeGrid;
	rootStore.dispatch({ type: GET_FILTER_BUNDLE_UPGRADE_INIZIATIVE, payload: rtn });
}
