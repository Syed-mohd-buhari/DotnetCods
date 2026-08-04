import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { LocationApi } from "../../../../Business/LookUp/LocationBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import { LocationGrid, LocationQueryObjectGrid, QueryResultDtoOfLocationDtoGrid } from '../../../../Model/LookUp/Location';

export async function GetLocationGrid(queryFilter?: LocationQueryObjectGrid) {
	setLoader("ADD", "GetLocationGrid");

	let result: QueryResultDtoOfLocationDtoGrid | null | undefined;
	let api = new LocationApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfLocationDtoGrid>>(() =>
				api.locationGetLocation(
					queryFilter?.id,
					queryFilter?.description,
					queryFilter?.opco,
					queryFilter?.locationType,
					queryFilter?.defaultValue,
					queryFilter?.sortBy,
					queryFilter?.isSortAscending,
					queryFilter?.page,
					queryFilter?.pageSize,
					queryFilter?.lastModified?.startDate,
					queryFilter?.lastModified?.endDate,
					queryFilter?.principalId,
					queryFilter?.deleted,
					queryFilter?.orphan,
					queryFilter?.lastModifiedBy
				)
			);
		} else {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfLocationDtoGrid>>(() => api.locationGetLocation());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as LocationGrid;
		rootStore.dispatch({ type: "GET_GRID_LOCATION", payload: rtn as LocationGrid });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_LOCATION", payload: { LookUpGridResult: result, filter: null } as LocationGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetLocationGrid");
}

export async function GetLocationGridALL() {
	setLoader("ADD", "GetLocationGridALL");

	let result: QueryResultDtoOfLocationDtoGrid | null | undefined;
	let api = new LocationApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfLocationDtoGrid>>(() => api.locationGetLocation());
		let rtn = { LookUpGridResult: result, filter: null } as LocationGrid;
		rootStore.dispatch({ type: "GET_GRID_LOCATION_ALL", payload: rtn as LocationGrid });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_LOCATION_ALL", payload: { LookUpGridResult: result, filter: null } as LocationGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetLocationGridALL");
}

export async function GetFilterColumLocation(columName: string, columValue: string, queryFilter?: LocationQueryObjectGrid) {
	// setLoader("ADD", "GetFilterColumLocation");

	let result: FilterValueDto[] | undefined;
	let api = new LocationApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.locationGetFilterResult(
				columName,
				columValue,
				queryFilter?.id,
				queryFilter?.description,
				queryFilter?.opco,
				queryFilter?.locationType,
				queryFilter?.defaultValue,
				queryFilter?.sortBy,
				queryFilter?.isSortAscending,
				queryFilter?.page,
				queryFilter?.pageSize,
				queryFilter?.lastModified?.startDate,
				queryFilter?.lastModified?.endDate,
				queryFilter?.principalId,
				queryFilter?.deleted,
				queryFilter?.orphan,
				queryFilter?.lastModifiedBy
			)
		);
	} else {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.locationGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LocationGrid;
	rootStore.dispatch({ type: "GET_FILTER_LOCATION", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumLocation");
}
