import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { AssetCategoryApi } from "../../../../Business/LookUp/AssetCategoryBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetAssetCategoryCreateResource() {
	setLoader("ADD", "GetAssetCategoryCreateResource");

	let api = new AssetCategoryApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.assetCategoryGetCreateResourceAssetCategories());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_ASSET_CATEGORY", payload: rtn });
	setLoader("REMOVE", "GetAssetCategoryCreateResource");
}

export async function CreatAssetCategory(data: TipologicaGridDto) {
	setLoader("ADD", "CreatAssetCategory");
	let api = new AssetCategoryApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.assetCategoryCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_ASSET_CATEGORY", payload: rtn });
	setLoader("REMOVE", "CreatAssetCategory");
	return rtn;
}
