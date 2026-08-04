import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { AssetCategoryApi } from "../../../../Business/LookUp/AssetCategoryBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import { QueryResultDtoOfAssetCategoryDtoGrid, AssetCategoryDtoGrid , AssetCategoryQueryObjectGrid, AssetCategoryGrid } from "../../../../Model/LookUp/AssetCategory";
// import { useDispatch } from 'react-redux'

export async function GetAssetCategoryGrid(queryFilter?: AssetCategoryQueryObjectGrid) {
	let result: QueryResultDtoOfAssetCategoryDtoGrid | null | undefined;
	let api = new AssetCategoryApi();
	setLoader("ADD", "GetAssetCategoryGrid");

	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfAssetCategoryDtoGrid>>(() =>
				api.assetCategoryGetAssetCategory(
					queryFilter?.id,
					queryFilter?.description,
					queryFilter?.assetClassId,
					queryFilter?.takeFromAssetTypeTable,
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
					
				)
			);
		} else {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfAssetCategoryDtoGrid>>(() => api.assetCategoryGetAssetCategory());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as AssetCategoryDtoGrid;
		rootStore.dispatch({ type: "GET_GRID_ASSET_CATEGORY", payload: rtn as AssetCategoryDtoGrid });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_ASSET_CATEGORY", payload: { LookUpGridResult: result, filter: null } as AssetCategoryGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetAssetCategoryGrid");
}

export async function GetAssetCategoryGridALL() {
	let result: QueryResultDtoOfAssetCategoryDtoGrid | null | undefined;
	let api = new AssetCategoryApi();
	setLoader("ADD", "GetAssetCategoryGridALL");

	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfAssetCategoryDtoGrid>>(() => api.assetCategoryGetAssetCategory());
		let rtn = { LookUpGridResult: result, filter: null } as AssetCategoryDtoGrid;
		rootStore.dispatch({ type: "GET_GRID_ASSET_CATEGORY_ALL", payload: rtn as AssetCategoryDtoGrid });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_ASSET_CATEGORY_ALL", payload: { LookUpGridResult: result, filter: null } as AssetCategoryGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetAssetCategoryGridALL");
}

export async function GetFilterColumAssetCategory(columName: string, columValue: string, queryFilter?: AssetCategoryQueryObjectGrid) {
	// setLoader("ADD", "GetFilterColumAssetCategory");

	let result: FilterValueDto[] | undefined;
	let api = new AssetCategoryApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.assetCategoryGetFilterResult(
				columName,
				columValue,
				queryFilter?.id,
				queryFilter?.description,
				queryFilter?.assetClassId,
				queryFilter?.takeFromAssetTypeTable,
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
				
			)
		);
	} else {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.assetCategoryGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as AssetCategoryGrid;
	rootStore.dispatch({ type: "GET_FILTER_ASSET_CATEGORY", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumAssetCategory");
}
