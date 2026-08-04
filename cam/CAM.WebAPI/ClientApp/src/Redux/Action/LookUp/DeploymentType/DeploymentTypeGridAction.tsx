import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { DeploymentTypeApi } from "../../../../Business/LookUp/DeploymentTypeBusiness";
import { TipologicaGridDtoRule, QueryResultDtoOfTipologicaGridDtoRule, TipologicheQueryObjectGridRule, LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetDeploymentTypeGrid(queryFilter?: TipologicheQueryObjectGridRule) {
	setLoader("ADD", "GetDeploymentTypeGrid");

	let result: QueryResultDtoOfTipologicaGridDtoRule | null | undefined;
	let api = new DeploymentTypeApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoRule>>(() =>
				api.deploymentTypeGetDeploymentType(
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
					queryFilter?.lastModifiedBy,
					queryFilter?.rule
				)
			);
		} else {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoRule>>(() => api.deploymentTypeGetDeploymentType());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDtoRule;
		rootStore.dispatch({ type: "GET_GRID_DEPLOYMENT_TYPE", payload: rtn as TipologicaGridDtoRule });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_DEPLOYMENT_TYPE", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetDeploymentTypeGrid");
}

export async function GetDeploymentTypeGridALL() {
	setLoader("ADD", "GetDeploymentTypeGridALL");

	let result: QueryResultDtoOfTipologicaGridDtoRule | null | undefined;
	let api = new DeploymentTypeApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfTipologicaGridDtoRule>>(() => api.deploymentTypeGetDeploymentType());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDtoRule;
		rootStore.dispatch({ type: "GET_GRID_DEPLOYMENT_TYPE_ALL", payload: rtn as TipologicaGridDtoRule });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_DEPLOYMENT_TYPE_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetDeploymentTypeGridALL");
}

export async function GetFilterColumDeploymentType(columName: string, columValue: string, queryFilter?: TipologicheQueryObjectGridRule) {
	// setLoader("ADD", "GetFilterColumDeploymentType");

	let result: FilterValueDto[] | undefined;
	let api = new DeploymentTypeApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.deploymentTypeGetFilterResult(
				columName,
				columValue,
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
				queryFilter?.lastModifiedBy,
				queryFilter?.rule
			)
		);
	} else {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.deploymentTypeGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
	rootStore.dispatch({ type: "GET_FILTER_DEPLOYMENT_TYPE", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumDeploymentType");
}
