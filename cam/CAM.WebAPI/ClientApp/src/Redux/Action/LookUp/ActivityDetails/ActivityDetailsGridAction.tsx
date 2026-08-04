import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { ActivityDetailsApi } from "../../../../Business/LookUp/ActivityDetailsBusiness";
import { ActivityDetailsDtoGrid, ActivityDetailsFilterDto } from "../../../../Model/LookUp/ActivityDetails";
import { LookUpGrid, QueryResultDtoOfTipologicaGridDto, TipologicaGridDto, TipologicaQueryDtoForVirtualized } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetActivityDetailGrid(queryFilter?: TipologicaQueryDtoForVirtualized) {
	setLoader("ADD", "GetActivityDetailGrid");
	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new ActivityDetailsApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() =>
				api.activityDetailsGetActivityDetails(
					queryFilter?.forVirtualized,
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
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() => api.activityDetailsGetActivityDetails());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_ACTIVITY_DETAILS", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_ACTIVITY_DETAILS", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetActivityDetailGrid");
}

export async function GetActivityDetailGridALL() {
	setLoader("ADD", "GetActivityDetailGridALL");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new ActivityDetailsApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() => api.activityDetailsGetActivityDetails());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_ACTIVITY_DETAILS_ALL", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_ACTIVITY_DETAILS_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetActivityDetailGridALL");
}

export async function GetFilterColumActivityDetail(columName: string, columValue: string, queryFilter?: TipologicaQueryDtoForVirtualized) {
	// setLoader("ADD", "GetFilterColumActivityDetail");

	let result: FilterValueDto[] | undefined;
	let api = new ActivityDetailsApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.activityDetailsGetFilterResult(
				columName,
				columValue,
				queryFilter?.forVirtualized,
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
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.activityDetailsGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
	rootStore.dispatch({ type: "GET_FILTER_ACTIVITY_DETAILS", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumActivityDetail");
}
