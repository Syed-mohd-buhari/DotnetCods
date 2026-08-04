import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { OperationalRiskApi } from "../../../../Business/LookUp/OperationalRiskBusiness";
import { OperationalRiskDto, QueryResultDtoOfOperationalRiskDto, OperationalRiskQueryDto, LookUpGridRisk } from "../../../../Model/LookUp/OperationalRisk";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetOperationalRiskGrid(queryFilter?: OperationalRiskQueryDto) {
	setLoader("ADD", "GetOperationalRiskGrid");

	let result: QueryResultDtoOfOperationalRiskDto | null | undefined;
	let api = new OperationalRiskApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfOperationalRiskDto>>(() =>
				api.operationalRiskGetOperationalRisk(
					queryFilter?.id,
					queryFilter?.severity,
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
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfOperationalRiskDto>>(() => api.operationalRiskGetOperationalRisk());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridRiskResult: result, filter: null } as OperationalRiskDto;
		rootStore.dispatch({ type: "GET_GRID_OPERATIONAL_RISK", payload: rtn as OperationalRiskDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_OPERATIONAL_RISK", payload: { LookUpGridRiskResult: result, filter: null } as LookUpGridRisk });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetOperationalRiskGrid");
}

export async function GetOperationalRiskGridALL() {
	setLoader("ADD", "GetOperationalRiskGridALL");

	let result: QueryResultDtoOfOperationalRiskDto | null | undefined;
	let api = new OperationalRiskApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfOperationalRiskDto>>(() => api.operationalRiskGetOperationalRisk());
		let rtn = { LookUpGridRiskResult: result, filter: null } as LookUpGridRisk;
		rootStore.dispatch({ type: "GET_GRID_OPERATIONAL_RISK_ALL", payload: rtn as LookUpGridRisk });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_OPERATIONAL_RISK_ALL", payload: { LookUpGridRiskResult: result, filter: null } as LookUpGridRisk });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetOperationalRiskGridALL");
}

export async function GetFilterColumOperationalRisk(columName: string, columValue: string, queryFilter?: OperationalRiskQueryDto) {
	// setLoader("ADD", "GetFilterColumOperationalRisk");

	let result: FilterValueDto[] | undefined;
	let api = new OperationalRiskApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.operationalRiskGetFilterResult(
				columName,
				columValue,
				queryFilter?.id,
				queryFilter?.severity,
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
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.operationalRiskGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridRiskResult: null } as LookUpGridRisk;
	rootStore.dispatch({ type: "GET_FILTER_OPERATIONAL_RISK", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumOperationalRisk");
}
