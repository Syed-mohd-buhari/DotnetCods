import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { SupportProviderApi } from "../../../../Business/LookUp/SupportProviderBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteSupportProvider(id: number) {
	setLoader("ADD", "deleteSupportProvider");
	let api = new SupportProviderApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.supportProviderDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_SUPPORT_PROVIDER", payload: rtn });
	setLoader("REMOVE", "deleteSupportProvider");
	return rtn;
}
