import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { RelatesToResourceApi } from "../../../../Business/LookUp/RelatesToResourceBusiness";
import { TipologicaGridDto, QueryResultDtoOfTipologicaGridDto, TipologicheQueryObjectGrid, LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetRelatesToResourceGrid(queryFilter?: TipologicheQueryObjectGrid) {
	setLoader("ADD", "GetRelatesToResourceGrid");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new RelatesToResourceApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() =>
				api.relatesToResourceGetRelatesToResource(
					queryFilter?.sortBy,
					queryFilter?.isSortAscending,
					queryFilter?.page,
					queryFilter?.pageSize,
					queryFilter?.lastModifiedStartDate,
					queryFilter?.lastModifiedEndDate,
					queryFilter?.principalId,
					queryFilter?.deleted,
					queryFilter?.orphan,
					queryFilter?.lastModifiedBy,
					queryFilter?.id,
					queryFilter?.description
				)
			);
		} else {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() => api.relatesToResourceGetRelatesToResource());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_RELATES_TO_RESOURCE", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_RELATES_TO_RESOURCE", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetRelatesToResourceGrid");
}

export async function GetRelatesToResourceGridALL() {
	setLoader("ADD", "GetRelatesToResourceGridALL");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new RelatesToResourceApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() => api.relatesToResourceGetRelatesToResource());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_RELATES_TO_RESOURCE_ALL", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_RELATES_TO_RESOURCE_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetRelatesToResourceGridALL");
}

export async function GetFilterColumRelatesToResource(columName: string, columValue: string, queryFilter?: TipologicheQueryObjectGrid) {
	// setLoader("ADD", "GetFilterColumRelatesToResource");

	let result: FilterValueDto[] | undefined;
	let api = new RelatesToResourceApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.relatesToResourceGetFilterResult(
				columName,
				columValue,
				queryFilter?.sortBy,
				queryFilter?.isSortAscending,
				queryFilter?.page,
				queryFilter?.pageSize,
				queryFilter?.lastModifiedStartDate,
				queryFilter?.lastModifiedEndDate,
				queryFilter?.principalId,
				queryFilter?.deleted,
				queryFilter?.orphan,
				queryFilter?.lastModifiedBy,
				queryFilter?.id,
				queryFilter?.description
			)
		);
	} else {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.relatesToResourceGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
	rootStore.dispatch({ type: "GET_FILTER_RELATES_TO_RESOURCE", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumRelatesToResource");
}
