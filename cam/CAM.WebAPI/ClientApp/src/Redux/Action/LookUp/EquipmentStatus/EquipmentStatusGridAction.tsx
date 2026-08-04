import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { EquipmentStatusApi } from "../../../../Business/LookUp/EquipmentStatusBusiness";
import { TipologicaGridDto, QueryResultDtoOfTipologicaGridDto, TipologicheQueryObjectGrid, LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetEquipmentStatusGrid(queryFilter?: TipologicheQueryObjectGrid) {
	setLoader("ADD", "GetEquipmentStatusGrid");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new EquipmentStatusApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() =>
				api.equipmentStatusGetEndOfSupportContract(
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
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() => api.equipmentStatusGetEndOfSupportContract());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_EQUIPMENT_STATUS", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_EQUIPMENT_STATUS", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetEquipmentStatusGrid");
}

export async function GetEquipmentStatusGridALL() {
	setLoader("ADD", "GetEquipmentStatusGridALL");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new EquipmentStatusApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDto>>(() => api.equipmentStatusGetEndOfSupportContract());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_EQUIPMENT_STATUS_ALL", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_EQUIPMENT_STATUS_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetEquipmentStatusGridALL");
}

export async function GetFilterColumEquipmentStatus(columName: string, columValue: string, queryFilter?: TipologicheQueryObjectGrid) {
	// setLoader("ADD", "GetFilterColumEquipmentStatus");

	let result: FilterValueDto[] | undefined;
	let api = new EquipmentStatusApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.equipmentStatusGetFilterResult(
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
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.equipmentStatusGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
	rootStore.dispatch({ type: "GET_FILTER_EQUIPMENT_STATUS", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumEquipmentStatus");
}
