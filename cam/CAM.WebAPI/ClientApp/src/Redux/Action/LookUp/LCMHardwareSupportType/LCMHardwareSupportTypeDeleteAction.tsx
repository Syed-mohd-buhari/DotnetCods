import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { LCMHardwareSupportTypeApi } from "../../../../Business/LookUp/LCMHardwareSupportTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteLCMHardwareSupportType(id: number) {
	let api = new LCMHardwareSupportTypeApi();
	setLoader("ADD", "deleteLCMHardwareSupportType");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.lCMHardwareSupportTypeDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_LCM_HW_SUPPORT_TYPE", payload: rtn });
	setLoader("REMOVE", "deleteLCMHardwareSupportType");
	return rtn;
}
