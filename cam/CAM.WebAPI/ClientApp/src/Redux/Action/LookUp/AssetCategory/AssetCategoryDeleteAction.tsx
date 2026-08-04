import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { AssetCategoryApi } from "../../../../Business/LookUp/AssetCategoryBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteAssetCategory(id: number) {
	let api = new AssetCategoryApi();
	setLoader("ADD", "deleteAssetCategory");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.assetCategoryDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_ASSET_CATEGORY", payload: rtn });
	setLoader("REMOVE", "deleteAssetCategory");
	return rtn;
}

export async function DeleteDeepAssetCategory(id: number) {
	let api = new AssetCategoryApi();
	setLoader("ADD", "DeleteDeepAssetCategory");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.assetCategoryDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_ASSET_CATEGORY", payload: rtn });
	setLoader("REMOVE", "DeleteDeepAssetCategory");
	return rtn;
}

export async function GetRelatedRecordsAssetCategory(id: number) {
	let api = new AssetCategoryApi();
	setLoader("ADD", "GetRelatedRecordsAssetCategory");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.assetCategoryGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsAssetCategory");
	return rtn;
}
