import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DeploymentStatusApi } from "../../../../Business/LookUp/DeploymentStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { DeploymentStatusDto } from "../../../../Model/LookUp/DeploymentStatus";
import { LookUpCreate, LookUpForDeploymentStatusCreate, LookUpGridForDeploymentStatus, TipologicaGridDtoRule } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetDeploymentStatusCreateResource() {
	setLoader("ADD", "GetDeploymentStatusCreateResource");

	let api = new DeploymentStatusApi();
	let createResource = await ApiCallWithErrorHandling<Promise<DeploymentStatusDto>>(() => api.deploymentStatusGetCreateResourceDeploymentStatus());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpForDeploymentStatusCreate;
	rootStore.dispatch({ type: "GET_CREATE_DEPLOYMENT_STATUS", payload: rtn });
	setLoader("REMOVE", "GetDeploymentStatusCreateResource");
}

export async function CreatDeploymentStatus(data: DeploymentStatusDto) {
	setLoader("ADD", "CreatDeploymentStatus");
	let api = new DeploymentStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deploymentStatusCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpForDeploymentStatusCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_DEPLOYMENT_STATUS", payload: rtn });
	setLoader("REMOVE", "CreatDeploymentStatus");
	return rtn;
}
