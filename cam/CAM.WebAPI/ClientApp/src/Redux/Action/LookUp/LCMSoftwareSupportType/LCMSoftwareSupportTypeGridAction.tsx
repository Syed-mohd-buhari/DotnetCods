import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { LCMSoftwareSupportTypeApi } from "../../../../Business/LookUp/LCMSoftwareSupportTypeBusiness";
import { LCMSoftwareSupportTypeQueryObjectGrid, LCMSoftwareSupportTypGrid, QueryResultDtoOfLCMSoftwareSupportTypeDtoGrid } from "../../../../Model/LookUp/LCMSoftwareSupportType";
import { TipologicaGridDto, QueryResultDtoOfTipologicaGridDto, TipologicheQueryObjectGrid, LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetLCMSoftwareSupportTypeGrid(queryFilter?: LCMSoftwareSupportTypeQueryObjectGrid) {
	setLoader("ADD", "GetLCMSoftwareSupportTypeGrid");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new LCMSoftwareSupportTypeApi();

	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() =>
				api.lCMSoftwareSupportTypeGetLCMSoftwareSupportType(
					queryFilter?.id,
					queryFilter?.description,
					queryFilter?.warranty,
					queryFilter?.oemSupport,
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
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfLCMSoftwareSupportTypeDtoGrid>>(() => api.lCMSoftwareSupportTypeGetLCMSoftwareSupportType());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { GridResult: result, filter: null } as LCMSoftwareSupportTypGrid;
		rootStore.dispatch({ type: "GET_GRID_LCM_SOFTWARE_SUPPORT_TYPE", payload: rtn as LCMSoftwareSupportTypGrid });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_LCM_SOFTWARE_SUPPORT_TYPE", payload: { GridResult: result, filter: null } as LCMSoftwareSupportTypGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetLCMSoftwareSupportTypeGrid");
}

export async function GetLCMSoftwareSupportTypeGridALL() {
	setLoader("ADD", "GetLCMSoftwareSupportTypeGridALL");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new LCMSoftwareSupportTypeApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfLCMSoftwareSupportTypeDtoGrid>>(() => api.lCMSoftwareSupportTypeGetLCMSoftwareSupportType());
		let rtn = { GridResultAll: result, filter: null } as LCMSoftwareSupportTypGrid;
		rootStore.dispatch({ type: "GET_GRID_LCM_SOFTWARE_SUPPORT_TYPE_ALL", payload: rtn as LCMSoftwareSupportTypGrid });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_LCM_SOFTWARE_SUPPORT_TYPE_ALL", payload: { GridResultAll: result, filter: null } as LCMSoftwareSupportTypGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetLCMSoftwareSupportTypeGridALL");
}

export async function GetFilterColumLCMSoftwareSupportType(columName: string, columValue: string, queryFilter?: LCMSoftwareSupportTypeQueryObjectGrid) {
	// setLoader("ADD", "GetFilterColumLCMSoftwareSupportType");

	let result: FilterValueDto[] | undefined;
	let api = new LCMSoftwareSupportTypeApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.lCMSoftwareSupportTypeGetFilterResult(
				columName,
				columValue,
				queryFilter?.id,
				queryFilter?.description,
				queryFilter?.warranty,
				queryFilter?.oemSupport,
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
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.lCMSoftwareSupportTypeGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, GridResult: null } as LCMSoftwareSupportTypGrid;
	rootStore.dispatch({ type: "GET_FILTER_LCM_SOFTWARE_SUPPORT_TYPE", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumLCMSoftwareSupportType");
}
