import React from "react";

import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { BuildConstructionApi } from "../../../../Business/LookUp/BuildConstructionBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function deleteBuildConstruction(id: number) {
	setLoader("ADD", "deleteBuildConstruction");
	let api = new BuildConstructionApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.buildConstructionDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_BUILD_CONSTRUCTION", payload: rtn });
	setLoader("REMOVE", "deleteBuildConstruction");
	return rtn;
}

export async function DeleteDeepBuildConstruction(id: number) {
	let api = new BuildConstructionApi();
	setLoader("ADD", "DeleteDeepBuildConstruction");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.buildConstructionDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_BUILD_CONSTRUCTION", payload: rtn });
	setLoader("REMOVE", "DeleteDeepBuildConstruction");
	return rtn;
}

export async function GetRelatedRecordsBuildConstruction(id: number) {
	let api = new BuildConstructionApi();
	setLoader("ADD", "GetRelatedRecordsBuildConstruction");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.buildConstructionGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsBuildConstruction");
	return rtn;
}
