import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { NFVIStatusApi } from "../../../../Business/LookUp/NFVIStatusBusiness";
import { TipologicaGridDto, QueryResultDtoOfTipologicaGridDto, TipologicheQueryObjectGrid, LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { NFVIStatusQueryObjectGrid } from "../../../../Model/LookUp/NFVIStatus";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetNFVIStatusGrid(queryFilter?: NFVIStatusQueryObjectGrid) {
	setLoader("ADD", "GetNFVIStatusGrid");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new NFVIStatusApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() =>
				api.nFVIStatusGetNFVIStatus(
					queryFilter?.id,
					queryFilter?.description,
					queryFilter?.color,
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
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() => api.nFVIStatusGetNFVIStatus());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_NFVI_STATUS", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_NFVI_STATUS", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetNFVIStatusGrid");
}

export async function GetNFVIStatusGridALL() {
	setLoader("ADD", "GetNFVIStatusGridALL");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new NFVIStatusApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() => api.nFVIStatusGetNFVIStatus());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_NFVI_STATUS_ALL", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_NFVI_STATUS_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetNFVIStatusGridALL");
}

export async function GetFilterColumNFVIStatus(columName: string, columValue: string, queryFilter?: NFVIStatusQueryObjectGrid) {
	// setLoader("ADD", "GetFilterColumNFVIStatus");

	let result: FilterValueDto[] | undefined;
	let api = new NFVIStatusApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.nFVIStatusGetFilterResult(
				columName,
				columValue,
				queryFilter?.id,
				queryFilter?.description,
				queryFilter?.color,
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
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.nFVIStatusGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
	rootStore.dispatch({ type: "GET_FILTER_NFVI_STATUS", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumNFVIStatus");
}
