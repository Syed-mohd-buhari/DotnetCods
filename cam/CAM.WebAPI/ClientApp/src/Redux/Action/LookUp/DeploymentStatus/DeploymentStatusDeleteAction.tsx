import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DeploymentStatusApi } from "../../../../Business/LookUp/DeploymentStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteDeploymentStatus(id: number) {
	setLoader("ADD", "deleteDeploymentStatus");
	let api = new DeploymentStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deploymentStatusDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_DEPLOYMENT_STATUS", payload: rtn });
	setLoader("REMOVE", "deleteDeploymentStatus");
	return rtn;
}

export async function DeleteDeepDeploymentStatus(id: number) {
	let api = new DeploymentStatusApi();
	setLoader("ADD", "DeleteDeepDeploymentStatus");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deploymentStatusDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_DEPLOYMENT_STATUS", payload: rtn });
	setLoader("REMOVE", "DeleteDeepDeploymentStatus");
	return rtn;
}

export async function GetRelatedRecordsDeploymentStatus(id: number) {
	let api = new DeploymentStatusApi();
	setLoader("ADD", "GetRelatedRecordsDeploymentStatus");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deploymentStatusGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsDeploymentStatus");
	return rtn;
}
