import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { BudgetAvailabilityApi } from "../../../../Business/LookUp/BudgetAvailabilityBusiness";
import { TipologicaGridDtoCombinationRule, QueryResultDtoOfTipologicaGridDtoCombinationRule, LookUpGrid, TipologicheQueryObjectGridCombinationRule, LookUpGridRule } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetBudgetAvailabilityGrid(queryFilter?: TipologicheQueryObjectGridCombinationRule) {
	setLoader("ADD", "GetBudgetAvailabilityGrid");

	let result: QueryResultDtoOfTipologicaGridDtoCombinationRule | null | undefined;
	let api = new BudgetAvailabilityApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoCombinationRule>>(() =>
				api.budgetAvailabilityGetBudgetAvailability(
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
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoCombinationRule>>(() => api.budgetAvailabilityGetBudgetAvailability());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDtoCombinationRule;
		rootStore.dispatch({ type: "GET_GRID_BUDGET_AVAILABILITY", payload: rtn as TipologicaGridDtoCombinationRule });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_BUDGET_AVAILABILITY", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetBudgetAvailabilityGrid");
}

export async function GetBudgetAvailabilityGridALL() {
	setLoader("ADD", "GetBudgetAvailabilityGridALL");

	let result: QueryResultDtoOfTipologicaGridDtoCombinationRule | null | undefined;
	let api = new BudgetAvailabilityApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoCombinationRule>>(() => api.budgetAvailabilityGetBudgetAvailability());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDtoCombinationRule;
		rootStore.dispatch({ type: "GET_GRID_BUDGET_AVAILABILITY_ALL", payload: rtn as TipologicaGridDtoCombinationRule });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_BUDGET_AVAILABILITY_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetBudgetAvailabilityGridALL");
}

export async function GetFilterColumBudgetAvailability(columName: string, columValue: string, queryFilter?: TipologicheQueryObjectGridCombinationRule) {
	// setLoader("ADD", "GetFilterColumBudgetAvailability");

	let result: FilterValueDto[] | undefined;
	let api = new BudgetAvailabilityApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.budgetAvailabilityGetFilterResult(
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
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.budgetAvailabilityGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGridRule;
	rootStore.dispatch({ type: "GET_FILTER_BUDGET_AVAILABILITY", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumBudgetAvailability");
}
