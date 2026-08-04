import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DeploymentStatusApi } from "../../../../Business/LookUp/DeploymentStatusBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { DeploymentStatusDto } from "../../../../Model/LookUp/DeploymentStatus";
import { LookUpEdit, LookUpForDeploymentStatusCreate, LookUpForDeploymentStatusEdit, TipologicaGridDtoRule } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetDeploymentStatusEditResource(id: number) {
	setLoader("ADD", "GetDeploymentStatusEditResource");
	let api = new DeploymentStatusApi();
	let createResource = await ApiCallWithErrorHandling<Promise<DeploymentStatusDto>>(() => api.deploymentStatusGetUpdateResourceDeploymentStatus(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpForDeploymentStatusEdit;
	rootStore.dispatch({ type: "GET_EDIT_DEPLOYMENT_STATUS", payload: rtn });
	setLoader("REMOVE", "GetDeploymentStatusEditResource");

	return rtn;
}

export async function EditDeploymentStatus(data: DeploymentStatusDto) {
	setLoader("ADD", "EditDeploymentStatus");
	let api = new DeploymentStatusApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deploymentStatusPut(data));
	let rtn = { ResultDtoEdit: result } as LookUpForDeploymentStatusEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_DEPLOYMENT_STATUS", payload: rtn });
	setLoader("REMOVE", "EditDeploymentStatus");
	return rtn;
}
