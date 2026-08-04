import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { DeploymentStatusApi } from "../../../../Business/LookUp/DeploymentStatusBusiness";
import { DeploymentStatusDtoGrid, DeploymentStatusQuery, QueryResultDtoOfDeploymentStatusDtoGrid } from "../../../../Model/LookUp/DeploymentStatus";
import { TipologicaGridDto, QueryResultDtoOfTipologicaGridDtoRule, TipologicheQueryObjectGridRule, LookUpGridForDeploymentStatus } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetDeploymentStatusGrid(queryFilter?: DeploymentStatusQuery) {
	setLoader("ADD", "GetDeploymentStatusGrid");

	let result: QueryResultDtoOfDeploymentStatusDtoGrid | null | undefined;
	let api = new DeploymentStatusApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfDeploymentStatusDtoGrid>>(() =>
				api.deploymentStatusGetDeploymentStatus(
					queryFilter?.deploymentStatusId,
					queryFilter?.deploymentStatusDescription,
					queryFilter?.rule,
					queryFilter?.plannedActivityResourceAllowed,
					queryFilter?.readOnlyPlannedActivity,
					queryFilter?.checkPlannedActivity,
					queryFilter?.defaultValue,
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
					queryFilter?.options
				)
			);
		} else {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfDeploymentStatusDtoGrid>>(() => api.deploymentStatusGetDeploymentStatus());
		}
		if (result?.items?.length === 0 || result?.totalItems === undefined) {
			rootStore.dispatch(setNotification({ message: "No results found", notifyType: NotifyType.error }));
		}
		let rtn = { LookUpGridResult: result, filter: null } as DeploymentStatusDtoGrid;
		rootStore.dispatch({ type: "GET_GRID_DEPLOYMENT_STATUS", payload: rtn as DeploymentStatusDtoGrid });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_DEPLOYMENT_STATUS", payload: { LookUpGridResult: result, filter: null } as LookUpGridForDeploymentStatus });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetDeploymentStatusGrid");
}

export async function GetDeploymentStatusGridALL() {
	setLoader("ADD", "GetDeploymentStatusGridALL");

	let result: QueryResultDtoOfDeploymentStatusDtoGrid | null | undefined;
	let api = new DeploymentStatusApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfDeploymentStatusDtoGrid>>(() => api.deploymentStatusGetDeploymentStatus());
		let rtn = { LookUpGridResult: result, filter: null } as DeploymentStatusDtoGrid;
		rootStore.dispatch({ type: "GET_GRID_DEPLOYMENT_STATUS_ALL", payload: rtn as DeploymentStatusDtoGrid });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_DEPLOYMENT_STATUS_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGridForDeploymentStatus });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetDeploymentStatusGridALL");
}

export async function GetFilterColumDeploymentStatus(columName: string, columValue: string, queryFilter?: DeploymentStatusQuery) {
	// setLoader("ADD", "GetFilterColumDeploymentStatus");

	let result: FilterValueDto[] | undefined;
	let api = new DeploymentStatusApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.deploymentStatusGetFilterResult(
				columName,
				columValue,
				queryFilter?.deploymentStatusId,
				queryFilter?.deploymentStatusDescription,
				queryFilter?.rule,
				queryFilter?.plannedActivityResourceAllowed,
				queryFilter?.readOnlyPlannedActivity,
				queryFilter?.checkPlannedActivity,
				queryFilter?.defaultValue,
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
				queryFilter?.options
			)
		);
	} else {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.deploymentStatusGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGridForDeploymentStatus;
	rootStore.dispatch({ type: "GET_FILTER_DEPLOYMENT_STATUS", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumDeploymentStatus");
}
