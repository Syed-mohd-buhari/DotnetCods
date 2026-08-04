import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { NFVIStatusApi } from "../../../../Business/LookUp/NFVIStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetNFVIStatusEditResource(id: number) {
	setLoader("ADD", "GetNFVIStatusEditResource");

	let api = new NFVIStatusApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.nFVIStatusGetUpdateResourceNFVIStatus(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_NFVI_STATUS", payload: rtn });
	setLoader("REMOVE", "GetNFVIStatusEditResource");

	return rtn;
}

export async function EditNFVIStatus(data: TipologicaGridDto) {
	setLoader("ADD", "EditNFVIStatus");
	let api = new NFVIStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVIStatusPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_NFVI_STATUS", payload: rtn });
	setLoader("REMOVE", "EditNFVIStatus");
	return rtn;
}
