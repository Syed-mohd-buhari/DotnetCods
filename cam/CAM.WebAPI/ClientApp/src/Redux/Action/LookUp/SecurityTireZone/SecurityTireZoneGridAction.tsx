import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { SecurityTireZoneApi } from "../../../../Business/LookUp/SecurityTireZoneBusiness";
import { TipologicaGridDto, QueryResultDtoOfTipologicaGridDto, TipologicheQueryObjectGrid, LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSecurityTireZoneGrid(queryFilter?: TipologicheQueryObjectGrid) {
	setLoader("ADD", "GetSecurityTireZoneGrid");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new SecurityTireZoneApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() =>
				api.securityTireZoneGetSecurityTireZone(
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
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() => api.securityTireZoneGetSecurityTireZone());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_SECURITY_TIRE_ZONE", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_SECURITY_TIRE_ZONE", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetSecurityTireZoneGrid");
}

export async function GetSecurityTireZoneGridALL() {
	setLoader("ADD", "GetSecurityTireZoneGridALL");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new SecurityTireZoneApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() => api.securityTireZoneGetSecurityTireZone());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_SECURITY_TIRE_ZONE_ALL", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_SECURITY_TIRE_ZONE_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetSecurityTireZoneGridALL");
}

export async function GetFilterColumSecurityTireZone(columName: string, columValue: string, queryFilter?: TipologicheQueryObjectGrid) {
	// setLoader("ADD", "GetFilterColumSecurityTireZone");

	let result: FilterValueDto[] | undefined;
	let api = new SecurityTireZoneApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.securityTireZoneGetFilterResult(
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
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.securityTireZoneGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
	rootStore.dispatch({ type: "GET_FILTER_SECURITY_TIRE_ZONE", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumSecurityTireZone");
}
