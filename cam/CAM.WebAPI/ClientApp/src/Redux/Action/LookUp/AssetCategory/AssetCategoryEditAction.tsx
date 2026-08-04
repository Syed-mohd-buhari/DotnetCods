import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { AssetCategoryApi } from "../../../../Business/LookUp/AssetCategoryBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetAssetCategoryEditResource(id: number) {
	setLoader("ADD", "GetAssetCategoryEditResource");

	let api = new AssetCategoryApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.assetCategoryGetUpdateResourceAssetCategories(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_ASSET_CATEGORY", payload: rtn });
	setLoader("REMOVE", "GetAssetCategoryEditResource");

	return rtn;
}

export async function EditAssetCategory(data: TipologicaGridDto) {
	setLoader("ADD", "EditAssetCategory");
	let api = new AssetCategoryApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.assetCategoryPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_ASSET_CATEGORY", payload: rtn });
	setLoader("REMOVE", "EditAssetCategory");
	return rtn;
}
