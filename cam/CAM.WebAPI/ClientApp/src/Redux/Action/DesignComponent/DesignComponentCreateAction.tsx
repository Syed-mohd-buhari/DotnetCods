import { type } from "os";
import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { DesignComponentApiFetchParamCreator, DesignComponentApi } from "../../../Business/DesignComponentBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { CREATE_DESIGN_COMPONENT, DesignComponentCreate, DesignComponentDtoCreate, GET_CREATE_DESIGN_COMPONENT } from "../../../Model/DesignComponent";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetDesignComponentCreateResource() {
	setLoader("ADD", "GetDesignComponentCreateResource");

	// const dispach = useDispatch();
	// ActionCenter<>
	// let test = await  ActionCenter<Promise<DesignComponentDtoCreate>>(() => api.designComponentGetCreateResourceDesignComponent());
	let api = new DesignComponentApi();

	let createResource = await ApiCallWithErrorHandling<Promise<DesignComponentDtoCreate>>(() => api.designComponentGetCreateResourceDesignComponent());
	let rtn = { ResultDtoCreate: null, DesignComponentDtoCreate: createResource } as DesignComponentCreate;
	rootStore.dispatch({ type: GET_CREATE_DESIGN_COMPONENT, payload: rtn });
	setLoader("REMOVE", "GetDesignComponentCreateResource");
	return rtn.DesignComponentDtoCreate;
}

export async function GetDesignComponentCreateResourceReducerLess() {
	setLoader("ADD", "GetDesignComponentCreateResource");

	let api = new DesignComponentApi();

	let createResource = await ApiCallWithErrorHandling<Promise<DesignComponentDtoCreate>>(() => api.designComponentGetCreateResourceDesignComponent());
	let rtn = { ResultDtoCreate: null, DesignComponentDtoCreate: createResource } as DesignComponentCreate;
	setLoader("REMOVE", "GetDesignComponentCreateResource");
	return rtn.DesignComponentDtoCreate;
}

export async function CreatDesignComponent(data: DesignComponentDtoCreate, forced?: boolean) {
	setLoader("ADD", "CreatDesignComponent");
	let api = new DesignComponentApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.designComponentCreate(data, forced));
	let rtn = { ResultDtoCreate: result, DesignComponentDtoCreate: null } as DesignComponentCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: CREATE_DESIGN_COMPONENT, payload: rtn });
	setLoader("REMOVE", "CreatDesignComponent");
	return rtn;
}
