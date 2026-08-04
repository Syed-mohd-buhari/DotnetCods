import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../Business/Common/CommonBusiness";
import { ViaExportHardwareApi } from "../../../Business/Report/ViaExportHardwareBusiness";
import { ViaExportQuery, QueryResultDtoOfViaExport, GET_GRID_VIA_EXPORT_HARDWARE, GET_FILTER_VIA_EXPORT_HARDWARE, ViaExportHardwareGrid, QueryResultDtoVai } from "../../../Model/ViaExport/ViaExport";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetViaExportHardwareGrid(queryFilter?: ViaExportQuery) {
	setLoader("ADD", "GetViaExportHardwareGrid");
	let result: QueryResultDtoVai | null | undefined;
	let api = new ViaExportHardwareApi();
	try {
		if (queryFilter !== null) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoVai>>(() =>
				api.viaExportHardwareGetViaExport(
					queryFilter ?? {} as ViaExportQuery
				)
			);
		} else {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoVai>>(() => api.viaExportHardwareGetViaExport(queryFilter ?? {} as ViaExportQuery));
		}
		let rtn = { ViaExportHardwareGridResult: result, filter: null } as ViaExportHardwareGrid;
		rootStore.dispatch({ type: GET_GRID_VIA_EXPORT_HARDWARE, payload: rtn });
		setLoader("REMOVE", "GetViaExportHardwareGrid");
		return result as QueryResultDtoVai
	} catch (error) {
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
		let rtn = { ViaExportHardwareGridResult: null, filter: null } as ViaExportHardwareGrid;
		rootStore.dispatch({ type: GET_GRID_VIA_EXPORT_HARDWARE, payload: rtn });
	}
	setLoader("REMOVE", "GetViaExportHardwareGrid");
}

export async function GetFilterColumViaExportHardware(columName: string, columValue: string, queryFilter?: ViaExportQuery) {
	let api = new ViaExportHardwareApi();
	let result: FilterValueDto[] | undefined;

		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.viaExportHardwareGetFilterResult(
				queryFilter ?? {}, columName,
				columValue,

			)
		);
	let rtn = { filter: result, ViaExportHardwareGridResult: null } as ViaExportHardwareGrid;
	rootStore.dispatch({ type: GET_FILTER_VIA_EXPORT_HARDWARE, payload: rtn });
}
