import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { ActivityDetailsApi } from "../../../../Business/LookUp/ActivityDetailsBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetActivityDetailsEditResource(id: number) {
	setLoader("ADD", "GetActivityDetailsEditResource");

	let api = new ActivityDetailsApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.activityDetailsGetUpdateResourceActivityDetails(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_ACTIVITY_DETAILS", payload: rtn });
	setLoader("REMOVE", "GetActivityDetailsEditResource");
	return rtn;
}

export async function EditActivityDetails(data: TipologicaGridDto) {
	setLoader("ADD", "EditActivityDetails");
	let api = new ActivityDetailsApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.activityDetailsPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_ACTIVITY_DETAILS", payload: rtn });
	setLoader("REMOVE", "EditActivityDetails");
	return rtn;
}
