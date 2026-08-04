import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { LCMSoftwareSupportTypeApi } from "../../../../Business/LookUp/LCMSoftwareSupportTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteLCMSoftwareSupportType(id: number) {
	setLoader("ADD", "deleteLCMSoftwareSupportType");
	let api = new LCMSoftwareSupportTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.lCMSoftwareSupportTypeDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_LCM_SOFTWARE_SUPPORT_TYPE", payload: rtn });
	setLoader("REMOVE", "deleteLCMSoftwareSupportType");
	return rtn;
}
