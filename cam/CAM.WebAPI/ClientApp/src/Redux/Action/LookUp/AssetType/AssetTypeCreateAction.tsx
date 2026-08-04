import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { AssetTypeApi } from "../../../../Business/LookUp/AssetTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { AssetTypeDto } from "../../../../Model/LookUp/AssetType";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetAssetTypeCreateResource() {
	setLoader("ADD", "GetAssetTypeCreateResource");

	let api = new AssetTypeApi();
	let createResource = await ApiCallWithErrorHandling<Promise<AssetTypeDto>>(() => api.assetTypeGetCreateResourceAssetType());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_ASSET_TYPE", payload: rtn });
	setLoader("REMOVE", "GetAssetTypeCreateResource");
}

export async function CreatAssetType(data: AssetTypeDto) {
	let api = new AssetTypeApi();
	setLoader("ADD", "CreatAssetType");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.assetTypeCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_ASSET_TYPE", payload: rtn });
	setLoader("REMOVE", "CreatAssetType");
	return rtn;
}
