import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { LCMPlannedActionResourceApi } from "../../../../Business/LookUp/LCMPlannedActionResourceBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteLCMPlannedActionResource(id: number) {
	setLoader("ADD", "deleteLCMPlannedActionResource");
	let api = new LCMPlannedActionResourceApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.lCMPlannedActionResourceDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_LCM_PLANNED_ACTION_RESOURCE", payload: rtn });
	setLoader("REMOVE", "deleteLCMPlannedActionResource");
	return rtn;
}
