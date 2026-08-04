import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { AssetTypeApi } from "../../../../Business/LookUp/AssetTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { AssetTypeDto } from "../../../../Model/LookUp/AssetType";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetAssetTypeEditResource(id: number) {
	setLoader("ADD", "GetAssetTypeEditResource");

	let api = new AssetTypeApi();
	let createResource = await ApiCallWithErrorHandling<Promise<AssetTypeDto>>(() => api.assetTypeGetUpdateResourceAssetType(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_ASSET_TYPE", payload: rtn });
	setLoader("REMOVE", "GetAssetTypeEditResource");

	return rtn;
}

export async function EditAssetType(data: AssetTypeDto) {
	setLoader("ADD", "EditAssetType");
	let api = new AssetTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.assetTypePut(data));
	let rtn = { ResultDtoEdit: result } as AssetTypeDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_ASSET_TYPE", payload: rtn });
	setLoader("REMOVE", "EditAssetType");
	return rtn;
}
