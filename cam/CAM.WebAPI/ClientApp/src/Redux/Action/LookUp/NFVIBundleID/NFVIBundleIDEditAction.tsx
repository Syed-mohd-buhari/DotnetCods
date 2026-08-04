import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NFVIBundleIDApi } from "../../../../Business/LookUp/NFVIBundleIDBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetNFVIBundleIDEditResource(id: number) {
	setLoader("ADD", "GetNFVIBundleIDEditResource");

	let api = new NFVIBundleIDApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.nFVIBundleIDGetUpdateResourceActivityStatus(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_NFVI_BUNDLE_ID", payload: rtn });
	setLoader("REMOVE", "GetNFVIBundleIDEditResource");

	return rtn;
}

export async function EditNFVIBundleID(data: TipologicaGridDto) {
	setLoader("ADD", "EditNFVIBundleID");
	let api = new NFVIBundleIDApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVIBundleIDPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_NFVI_BUNDLE_ID", payload: rtn });
	setLoader("REMOVE", "EditNFVIBundleID");
	return rtn;
}
