import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { HardwareTypeApi } from "../../../../Business/LookUp/HardwareTypeBusiness";
import { TipologicaGridDto, QueryResultDtoOfTipologicaGridDto, TipologicheQueryObjectGrid, LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetHardwareTypeGrid(queryFilter?: TipologicheQueryObjectGrid) {
	setLoader("ADD", "GetHardwareTypeGrid");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new HardwareTypeApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() =>
				api.hardwareTypeGetHardwareType(
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
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() => api.hardwareTypeGetHardwareType());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_HARDWARE_TYPE", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_HARDWARE_TYPE", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetHardwareTypeGrid");
}

export async function GetHardwareTypeGridALL() {
	setLoader("ADD", "GetHardwareTypeGridALL");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new HardwareTypeApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() => api.hardwareTypeGetHardwareType());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_HARDWARE_TYPE_ALL", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_HARDWARE_TYPE_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetHardwareTypeGridALL");
}

export async function GetFilterColumHardwareType(columName: string, columValue: string, queryFilter?: TipologicheQueryObjectGrid) {
	// setLoader("ADD", "GetFilterColumHardwareType");

	let result: FilterValueDto[] | undefined;
	let api = new HardwareTypeApi();

	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.hardwareTypeGetFilterResult(
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
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.hardwareTypeGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
	rootStore.dispatch({ type: "GET_FILTER_HARDWARE_TYPE", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumHardwareType");
}
