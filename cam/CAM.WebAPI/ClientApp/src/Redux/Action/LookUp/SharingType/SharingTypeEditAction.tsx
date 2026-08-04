import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SharingTypeApi } from "../../../../Business/LookUp/SharingTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSharingTypeEditResource(id: number) {
	setLoader("ADD", "GetSharingTypeEditResource");

	let api = new SharingTypeApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.sharingTypeGetUpdateResourceSharingType(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_SHARING_TYPE", payload: rtn });
	setLoader("REMOVE", "GetSharingTypeEditResource");

	return rtn;
}

export async function EditSharingType(data: TipologicaGridDto) {
	setLoader("ADD", "EditSharingType");
	let api = new SharingTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.sharingTypePut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_SHARING_TYPE", payload: rtn });
	setLoader("REMOVE", "EditSharingType");
	return rtn;
}
