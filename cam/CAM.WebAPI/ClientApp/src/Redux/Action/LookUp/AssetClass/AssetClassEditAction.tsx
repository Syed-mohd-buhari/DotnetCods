import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { AssetClassApi } from "../../../../Business/LookUp/AssetClassBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetAssetClassEditResource(id: number) {
	setLoader("ADD", "GetAssetClassEditResource");

	let api = new AssetClassApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.assetClassGetUpdateResourceAssetClass(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_ASSET_CLASS", payload: rtn });
	setLoader("REMOVE", "GetAssetClassEditResource");

	return rtn;
}

export async function EditAssetClass(data: TipologicaGridDto) {
	setLoader("ADD", "EditAssetClass");
	let api = new AssetClassApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.assetClassPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_ASSET_CLASS", payload: rtn });
	setLoader("REMOVE", "EditAssetClass");
	return rtn;
}
