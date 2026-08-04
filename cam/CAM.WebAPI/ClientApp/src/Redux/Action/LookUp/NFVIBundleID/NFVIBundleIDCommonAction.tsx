import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NFVIBundleIDApi } from "../../../../Business/LookUp/NFVIBundleIDBusiness";
import { ChangeGridOrderDto, ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function ChangeGridNFVIBundleID(data: Array<ChangeGridOrderDto>) {
	setLoader("ADD", "ChangeGridNFVIBundleID");

	let api = new NFVIBundleIDApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVIBundleIDChangeGridOrderNFVIBundleID(data));
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	setLoader("REMOVE", "ChangeGridNFVIBundleID");

	return result;
}
