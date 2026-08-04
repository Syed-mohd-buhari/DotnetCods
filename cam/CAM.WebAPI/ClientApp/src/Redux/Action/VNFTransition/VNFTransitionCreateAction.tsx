import { type } from "os";
import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { VNFTransitionApiFetchParamCreator, VNFTransitionApi } from "../../../Business/VNFTransitionBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { CREATE_VNF_TRANSITION, VNFTransitionCreate, VnfTransitionDtoCreate, GET_CREATE_VNF_TRANSITION } from "../../../Model/VNFTransition";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetVNFTransitionCreateResource() {
	setLoader("ADD", "GetVNFTransitionCreateResource");

	// const dispach = useDispatch();
	// ActionCenter<>
	// let test = await  ActionCenter<Promise<VNFTransitionDtoCreate>>(() => api.vNFTransitionGetCreateResourceVNFTransition());
	let api = new VNFTransitionApi();

	let createResource = await ApiCallWithErrorHandling<Promise<VnfTransitionDtoCreate>>(() => api.vNFTransitionGetCreateResourceVNFTransition());
	let rtn = { ResultDtoCreate: null, VNFTransitionDtoCreate: createResource } as VNFTransitionCreate;
	rootStore.dispatch({ type: GET_CREATE_VNF_TRANSITION, payload: rtn });
	setLoader("REMOVE", "GetVNFTransitionCreateResource");
}

export async function CreatVNFTransition(data: VnfTransitionDtoCreate, forced?: boolean) {
	setLoader("ADD", "CreatVNFTransition");
	let api = new VNFTransitionApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.vNFTransitionCreate(data, forced));
	let rtn = { ResultDtoCreate: result, VNFTransitionDtoCreate: null } as VNFTransitionCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: CREATE_VNF_TRANSITION, payload: rtn });
	setLoader("REMOVE", "CreatVNFTransition");
	return rtn;
}
