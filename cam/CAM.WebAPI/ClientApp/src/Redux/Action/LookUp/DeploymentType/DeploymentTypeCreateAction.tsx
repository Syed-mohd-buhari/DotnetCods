import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DeploymentTypeApi } from "../../../../Business/LookUp/DeploymentTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetDeploymentTypeCreateResource() {
	setLoader("REMOVE", "GetDeploymentTypeCreateResource");

	let api = new DeploymentTypeApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.deploymentTypeGetCreateResourceDeploymentType());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_DEPLOYMENT_TYPE", payload: rtn });
	setLoader("REMOVE", "GetDeploymentTypeCreateResource");
}

export async function CreatDeploymentType(data: TipologicaGridDto) {
	setLoader("ADD", "CreatDeploymentType");
	let api = new DeploymentTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deploymentTypeCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_DEPLOYMENT_TYPE", payload: rtn });
	setLoader("REMOVE", "CreatDeploymentType");
	return rtn;
}
