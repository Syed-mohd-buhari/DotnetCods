import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { OperatingSystemApi } from "../../../../Business/LookUp/OperatingSystemBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function deleteOperatingSystem(id: number) {
	setLoader("ADD", "deleteOperatingSystem");
	let api = new OperatingSystemApi();

	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.operatingSystemDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_OPERATING_SYSTEM", payload: rtn });
	setLoader("REMOVE", "deleteOperatingSystem");
	return rtn;
}

export async function DeleteDeepOperatingSystem(id: number) {
	let api = new OperatingSystemApi();
	setLoader("ADD", "DeleteDeepOperatingSystem");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.operatingSystemDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_OPERATING_SYSTEM", payload: rtn });
	setLoader("REMOVE", "DeleteDeepOperatingSystem");
	return rtn;
}

export async function GetRelatedRecordsOperatingSystem(id: number) {
	let api = new OperatingSystemApi();
	setLoader("ADD", "GetRelatedRecordsOperatingSystem");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.operatingSystemGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsOperatingSystem");
	return rtn;
}
