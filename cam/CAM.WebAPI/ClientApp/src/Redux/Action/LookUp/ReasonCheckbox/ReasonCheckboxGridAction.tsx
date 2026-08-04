import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { ReasonCheckboxResourceApi } from "../../../../Business/LookUp/ReasonCheckboxBusiness";
import { TipologicaGridDto, QueryResultDtoOfTipologicaGridDto, LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { ReasonCheckboxQueryObjectGrid } from "../../../../Model/LookUp/ReasonCheckbox";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetReasonCheckboxGrid(queryFilter?: ReasonCheckboxQueryObjectGrid) {
	setLoader("ADD", "GetReasonCheckboxGrid");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new ReasonCheckboxResourceApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() =>
				api.reasonCheckboxResourceGetReasonCheckboxResource(
					queryFilter?.id,
					queryFilter?.description,
					queryFilter?.isHardware,
					queryFilter?.isSoftware,
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
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() => api.reasonCheckboxResourceGetReasonCheckboxResource());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_REASON_CHECKBOX", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_REASON_CHECKBOX", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetReasonCheckboxGrid");
}

export async function GetReasonCheckboxGridALL() {
	setLoader("ADD", "GetReasonCheckboxGridALL");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new ReasonCheckboxResourceApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() => api.reasonCheckboxResourceGetReasonCheckboxResource());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_REASON_CHECKBOX_ALL", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_REASON_CHECKBOX_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetReasonCheckboxGridALL");
}

export async function GetFilterColumReasonCheckbox(columName: string, columValue: string, queryFilter?: ReasonCheckboxQueryObjectGrid) {
	// setLoader("ADD", "GetFilterColumReasonCheckbox");

	let result: FilterValueDto[] | undefined;
	let api = new ReasonCheckboxResourceApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.reasonCheckboxResourceGetFilterResult(
				columName,
				columValue,
				queryFilter?.id,
				queryFilter?.description,
				queryFilter?.isHardware,
				queryFilter?.isSoftware,
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
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.reasonCheckboxResourceGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
	rootStore.dispatch({ type: "GET_FILTER_REASON_CHECKBOX", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumReasonCheckbox");
}
