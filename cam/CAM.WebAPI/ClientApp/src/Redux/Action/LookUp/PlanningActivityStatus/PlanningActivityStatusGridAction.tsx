import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { PlanningActivityStatusApi } from "../../../../Business/LookUp/PlanningActivityStatusBusiness";
import {
	TipologicaGridDtoProjectStatusCombinationRule,
	QueryResultDtoOfTipologicaGridDtoProjectStatusCombinationRule,
	TipologicheQueryObjectGridProjectStatusCombinationRule,
	LookUpGrid,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetPlanningActivityStatusGrid(queryFilter?: TipologicheQueryObjectGridProjectStatusCombinationRule) {
	setLoader("ADD", "GetPlanningActivityStatusGrid");

	let result: QueryResultDtoOfTipologicaGridDtoProjectStatusCombinationRule | null | undefined;
	let api = new PlanningActivityStatusApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoProjectStatusCombinationRule>>(() =>
				api.planningActivityStatusGetPlanningActivityStatus(
					queryFilter?.projectStatusCombinationRule,
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
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoProjectStatusCombinationRule>>(() => api.planningActivityStatusGetPlanningActivityStatus());
		}

		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDtoProjectStatusCombinationRule;
		rootStore.dispatch({ type: "GET_GRID_PLANNING_ACTIVITY_STATUS", payload: rtn as TipologicaGridDtoProjectStatusCombinationRule });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_PLANNING_ACTIVITY_STATUS", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetPlanningActivityStatusGrid");
}

export async function GetPlanningActivityStatusGridALL() {
	setLoader("ADD", "GetPlanningActivityStatusGridALL");

	let result: QueryResultDtoOfTipologicaGridDtoProjectStatusCombinationRule | null | undefined;
	let api = new PlanningActivityStatusApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoProjectStatusCombinationRule>>(() => api.planningActivityStatusGetPlanningActivityStatus());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDtoProjectStatusCombinationRule;
		rootStore.dispatch({ type: "GET_GRID_PLANNING_ACTIVITY_STATUS_ALL", payload: rtn as TipologicaGridDtoProjectStatusCombinationRule });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_PLANNING_ACTIVITY_STATUS_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetPlanningActivityStatusGridALL");
}

export async function GetFilterColumPlanningActivityStatus(columName: string, columValue: string, queryFilter?: TipologicheQueryObjectGridProjectStatusCombinationRule) {
	// setLoader("ADD", "GetFilterColumPlanningActivityStatus");

	let result: FilterValueDto[] | undefined;
	let api = new PlanningActivityStatusApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.planningActivityStatusGetFilterResult(
				columName,
				columValue,
				queryFilter?.projectStatusCombinationRule,
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
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.planningActivityStatusGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
	rootStore.dispatch({ type: "GET_FILTER_PLANNING_ACTIVITY_STATUS", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumPlanningActivityStatus");
}
