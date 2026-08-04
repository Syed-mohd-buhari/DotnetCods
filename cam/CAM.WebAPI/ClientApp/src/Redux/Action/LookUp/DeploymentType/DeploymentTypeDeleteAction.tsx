import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DeploymentTypeApi } from "../../../../Business/LookUp/DeploymentTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteDeploymentType(id: number) {
	setLoader("ADD", "deleteDeploymentType");
	let api = new DeploymentTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deploymentTypeDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_DEPLOYMENT_TYPE", payload: rtn });
	setLoader("REMOVE", "deleteDeploymentType");
	return rtn;
}

export async function DeleteDeepDeploymentType(id: number) {
	let api = new DeploymentTypeApi();
	setLoader("ADD", "DeleteDeepDeploymentType");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deploymentTypeDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_DEPLOYMENT_TYPE", payload: rtn });
	setLoader("REMOVE", "DeleteDeepDeploymentType");
	return rtn;
}

export async function GetRelatedRecordsDeploymentType(id: number) {
	let api = new DeploymentTypeApi();
	setLoader("ADD", "GetRelatedRecordsDeploymentType");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deploymentTypeGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsDeploymentType");
	return rtn;
}
