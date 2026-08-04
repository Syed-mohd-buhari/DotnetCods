import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { NFVIBundleIDApi } from "../../../../Business/LookUp/NFVIBundleIDBusiness";
import { TipologicaGridDto, LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { NFVIBundleIDQueryObjectGrid, QueryResultDtoOfNFVIBundleIDDtoGrid } from "../../../../Model/LookUp/NFVIBundleId";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetNFVIBundleIDGrid(queryFilter?: NFVIBundleIDQueryObjectGrid) {
	setLoader("ADD", "GetNFVIBundleIDGrid");

	let result: QueryResultDtoOfNFVIBundleIDDtoGrid | null | undefined;
	let api = new NFVIBundleIDApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfNFVIBundleIDDtoGrid>>(() =>
				api.nFVIBundleIDGetNFVIBundleID(
					queryFilter?.id,
					queryFilter?.description,
					queryFilter?.order,
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
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfNFVIBundleIDDtoGrid>>(() => api.nFVIBundleIDGetNFVIBundleID());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_NFVI_BUNDLE_ID", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_NFVI_BUNDLE_ID", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetNFVIBundleIDGrid");
}

export async function GetNFVIBundleIDGridALL() {
	setLoader("ADD", "GetNFVIBundleIDGridALL");

	let result: QueryResultDtoOfNFVIBundleIDDtoGrid | null | undefined;
	let api = new NFVIBundleIDApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfNFVIBundleIDDtoGrid>>(() => api.nFVIBundleIDGetNFVIBundleID());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_NFVI_BUNDLE_ID_ALL", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_NFVI_BUNDLE_ID_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetNFVIBundleIDGridALL");
}

export async function GetFilterColumNFVIBundleID(columName: string, columValue: string, queryFilter?: NFVIBundleIDQueryObjectGrid) {
	// setLoader("ADD", "GetFilterColumNFVIBundleID");

	let result: FilterValueDto[] | undefined;
	let api = new NFVIBundleIDApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.nFVIBundleIDGetFilterResult(
				columName,
				columValue,
				queryFilter?.id,
				queryFilter?.description,
				queryFilter?.order,
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
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.nFVIBundleIDGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
	rootStore.dispatch({ type: "GET_FILTER_NFVI_BUNDLE_ID", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumNFVIBundleID");
}
