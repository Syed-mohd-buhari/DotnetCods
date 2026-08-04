import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { AssetTypeApi } from "../../../../Business/LookUp/AssetTypeBusiness";
import { AssetTypeQueryObjectGrid, QueryResultDtoOfAssetTypeDtoGrid } from "../../../../Model/LookUp/AssetType";
import { TipologicaGridDto, QueryResultDtoOfTipologicaGridDto, LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetAssetTypeGrid(queryFilter?: AssetTypeQueryObjectGrid) {
	setLoader("ADD", "GetAssetTypeGrid");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new AssetTypeApi();
	try {
		if (queryFilter !== null && queryFilter !== undefined) {
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfAssetTypeDtoGrid>>(() =>
				api.assetTypeGetAssetType(
					queryFilter?.id,
					queryFilter?.description,
					queryFilter?.assetCategoryId,
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
			result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfAssetTypeDtoGrid>>(() => api.assetTypeGetAssetType());
		}
		// if (result?.items?.length === 0 || result?.totalItems === undefined) {
		//     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
		// }
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_ASSET_TYPE", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_ASSET_TYPE", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetAssetTypeGrid");
}

export async function GetAssetTypeGridALL() {
	setLoader("ADD", "GetAssetTypeGridALL");

	let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
	let api = new AssetTypeApi();
	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfAssetTypeDtoGrid>>(() => api.assetTypeGetAssetType());
		let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
		rootStore.dispatch({ type: "GET_GRID_ASSET_TYPE_ALL", payload: rtn as TipologicaGridDto });
	} catch (error) {
		rootStore.dispatch({ type: "GET_GRID_ASSET_TYPE_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetAssetTypeGridALL");
}

export async function GetFilterColumAssetType(columName: string, columValue: string, queryFilter?: AssetTypeQueryObjectGrid) {
	// setLoader("ADD", "GetFilterColumAssetType");

	let result: FilterValueDto[] | undefined;
	let api = new AssetTypeApi();
	if (queryFilter !== null && queryFilter !== undefined) {
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
			api.assetTypeGetFilterResult(
				columName,
				columValue,
				queryFilter?.id,
				queryFilter?.description,
				queryFilter?.assetCategoryId,
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
		result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.assetTypeGetFilterResult(columName, columValue));
	}
	let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
	rootStore.dispatch({ type: "GET_FILTER_ASSET_TYPE", payload: rtn });
	// setLoader("REMOVE", "GetFilterColumAssetType");
}
