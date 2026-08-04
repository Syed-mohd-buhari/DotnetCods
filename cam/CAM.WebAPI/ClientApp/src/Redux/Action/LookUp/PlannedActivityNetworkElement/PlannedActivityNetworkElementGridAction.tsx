import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { PlannedActivityNetworkElementApi } from "../../../../Business/LookUp/PlannedActivityNetworkElementBusiness";
import { TipologicaGridDto, QueryResultDtoOfTipologicaGridDto, LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { PlannedActivityNetworkElementQueryObjectGrid, QueryResultDtoOfPlannedActivityNetworkElementDtoGrid } from "../../../../Model/LookUp/PlannedActivityNetworkElement";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetPlannedActivityNetworkElementGrid(queryFilter?: PlannedActivityNetworkElementQueryObjectGrid) {
	setLoader("ADD", "GetPlannedActivityNetworkElementGrid");

	let result: QueryResultDtoOfPlannedActivityNetworkElementDtoGrid | null | undefined;
	let api = new PlannedActivityNetworkElementApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfPlannedActivityNetworkElementDtoGrid>>(() =>
				api.plannedActivityNetworkElementGetPlannedActivityNetworkElement(
					queryFilter?.plannedActivityResourceId,
					queryFilter?.plannedActivityNetworkElementId,
					queryFilter?.plannedActivityResourceDescription,
					queryFilter?.plannedActivityNetworkElementDescription,
					queryFilter?.driverText,
					queryFilter?.benefitText,
					queryFilter?.forCreate,
					queryFilter?.forEdit,
					queryFilter?.rule,
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
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfPlannedActivityNetworkElementDtoGrid>>(() => api.plannedActivityNetworkElementGetPlannedActivityNetworkElement());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_PLANNED_ACTIVITY_NETWORK_ELEMENT", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_PLANNED_ACTIVITY_NETWORK_ELEMENT", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetPlannedActivityNetworkElementGrid");
}

export async function GetPlannedActivityNetworkElementGridALL() {
	setLoader("ADD", "GetPlannedActivityNetworkElementGridALL");

	let result: QueryResultDtoOfPlannedActivityNetworkElementDtoGrid | null | undefined;
	let api = new PlannedActivityNetworkElementApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfPlannedActivityNetworkElementDtoGrid>>(() => api.plannedActivityNetworkElementGetPlannedActivityNetworkElement());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_PLANNED_ACTIVITY_NETWORK_ELEMENT_ALL", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_PLANNED_ACTIVITY_NETWORK_ELEMENT_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetPlannedActivityNetworkElementGridALL");
}

export async function GetFilterColumPlannedActivityNetworkElement(columName: string, columValue: string, queryFilter?: PlannedActivityNetworkElementQueryObjectGrid) {
	// setLoader("ADD", "GetFilterColumPlannedActivityNetworkElement");

	let result: FilterValueDto[] | undefined;
	let api = new PlannedActivityNetworkElementApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.plannedActivityNetworkElementGetFilterResult(
				columName,
				columValue,
				queryFilter?.plannedActivityResourceId,
				queryFilter?.plannedActivityNetworkElementId,
				queryFilter?.plannedActivityResourceDescription,
				queryFilter?.plannedActivityNetworkElementDescription,
				queryFilter?.driverText,
				queryFilter?.benefitText,
				queryFilter?.forCreate,
				queryFilter?.forEdit,
				queryFilter?.rule,
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
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.plannedActivityNetworkElementGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
	rootStore.dispatch({ type: "GET_FILTER_PLANNED_ACTIVITY_NETWORK_ELEMENT", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumPlannedActivityNetworkElement");
}
