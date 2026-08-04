import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { AssetTypeApi } from "../../../../Business/LookUp/AssetTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteAssetType(id: number) {
	setLoader("ADD", "deleteAssetType");
	let api = new AssetTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.assetTypeDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_ASSET_TYPE", payload: rtn });
	setLoader("REMOVE", "deleteAssetType");
	return rtn;
}

export async function DeleteDeepAssetType(id: number) {
	let api = new AssetTypeApi();
	setLoader("ADD", "DeleteDeepAssetType");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.assetTypeDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_ASSET_TYPE", payload: rtn });
	setLoader("REMOVE", "DeleteDeepAssetType");
	return rtn;
}

export async function GetRelatedRecordsAssetType(id: number) {
	let api = new AssetTypeApi();
	setLoader("ADD", "GetRelatedRecordsAssetType");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.assetTypeGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsAssetType");
	return rtn;
}
