import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { ActivityStatusApi } from "../../../../Business/LookUp/ActivityStatusBusiness";
import { TipologicaGridDtoRule, QueryResultDtoOfTipologicaGridDtoRule, TipologicheQueryObjectGrid, LookUpGrid, TipologicheQueryObjectGridCombinationRule } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetActivityStatusGrid(queryFilter?: TipologicheQueryObjectGridCombinationRule) {
	let result: QueryResultDtoOfTipologicaGridDtoRule | null | undefined;
	let api = new ActivityStatusApi();
	setLoader("ADD", "GetActivityStatusGrid");

	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoRule>>(() =>
				api.activityStatusGetActivityStatus(
					queryFilter?.projectStatusCombinationRule,
					queryFilter?.rule,
					queryFilter?.id,
					queryFilter?.description,
					queryFilter?.sortBy,
					queryFilter?.isSortAscending,
					queryFilter?.page,
					queryFilter?.pageSize,
					queryFilter?.lastModifiedStartDate,
					queryFilter?.lastModifiedEndDate,
					queryFilter?.principalId,
					queryFilter?.deleted,
					queryFilter?.orphan,
					queryFilter?.lastModifiedBy
				)
			);
		} else {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoRule>>(() => api.activityStatusGetActivityStatus());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDtoRule;
		rootStore.dispatch({ type: "GET_GRID_ACTIVITY_STATUS", payload: rtn as TipologicaGridDtoRule });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_ACTIVITY_STATUS", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetActivityStatusGrid");
}

export async function GetActivityStatusGridALL() {
	setLoader("ADD", "GetActivityStatusGridALL");

	let result: QueryResultDtoOfTipologicaGridDtoRule | null | undefined;
	let api = new ActivityStatusApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoRule>>(() => api.activityStatusGetActivityStatus());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDtoRule;
		rootStore.dispatch({ type: "GET_GRID_ACTIVITY_STATUS_ALL", payload: rtn as TipologicaGridDtoRule });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_ACTIVITY_STATUS_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetActivityStatusGridALL");
}

export async function GetFilterColumActivityStatus(columName: string, columValue: string, queryFilter?: TipologicheQueryObjectGridCombinationRule) {
	let result: FilterValueDto[] | undefined;
	let api = new ActivityStatusApi();
	// setLoader("ADD", "GetFilterColumActivityStatus");

	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.activityStatusGetFilterResult(
				columName,
				columValue,
				queryFilter?.projectStatusCombinationRule,
				queryFilter?.rule,
				queryFilter?.id,
				queryFilter?.description,
				queryFilter?.sortBy,
				queryFilter?.isSortAscending,
				queryFilter?.page,
				queryFilter?.pageSize,
				queryFilter?.lastModifiedStartDate,
				queryFilter?.lastModifiedEndDate,
				queryFilter?.principalId,
				queryFilter?.deleted,
				queryFilter?.orphan,
				queryFilter?.lastModifiedBy
			)
		);
	} else {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.activityStatusGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
	rootStore.dispatch({ type: "GET_FILTER_ACTIVITY_STATUS", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumActivityStatus");
}
