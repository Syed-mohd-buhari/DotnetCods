import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { DeploymentTypeApi } from "../../../../Business/LookUp/DeploymentTypeBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetDeploymentTypeEditResource(id: number) {
	setLoader("ADD", "GetDeploymentTypeEditResource");

	let api = new DeploymentTypeApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.deploymentTypeGetUpdateResourceDeploymentType(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_DEPLOYMENT_TYPE", payload: rtn });
	setLoader("REMOVE", "GetDeploymentTypeEditResource");

	return rtn;
}

export async function EditDeploymentType(data: TipologicaGridDto) {
	setLoader("ADD", "EditDeploymentType");
	let api = new DeploymentTypeApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.deploymentTypePut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_DEPLOYMENT_TYPE", payload: rtn });
	setLoader("REMOVE", "EditDeploymentType");
	return rtn;
}
