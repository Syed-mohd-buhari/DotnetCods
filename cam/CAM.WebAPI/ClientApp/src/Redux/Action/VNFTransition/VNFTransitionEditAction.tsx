import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { VNFTransitionApiFetchParamCreator, VNFTransitionApi } from "../../../Business/VNFTransitionBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { VNFTransitionDtoUpdate, VNFTransitionEdit, EDIT_VNF_TRANSITION, GET_EDIT_VNF_TRANSITION } from "../../../Model/VNFTransition";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetVNFTransitionEditResource(id: number) {
	setLoader("ADD", "GetVNFTransitionEditResource");

	// const dispach = useDispatch();
	let api = new VNFTransitionApi();
	let createResource = await ApiCallWithErrorHandling<Promise<VNFTransitionDtoUpdate>>(() => api.vNFTransitionGetUpdateResourceVNFTransition(id));
	let rtn = { VNFTransitionDtoEdit: createResource } as VNFTransitionEdit;
	rootStore.dispatch({ type: GET_EDIT_VNF_TRANSITION, payload: rtn });
	setLoader("REMOVE", "GetVNFTransitionEditResource");

	return rtn;
}

export async function EditVNFTransition(data: VNFTransitionDtoUpdate, forced?: boolean) {
	setLoader("ADD", "EditVNFTransition");
	let api = new VNFTransitionApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.vNFTransitionPut(data, forced));
	let rtn = { ResultDtoEdit: result } as VNFTransitionEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: EDIT_VNF_TRANSITION, payload: rtn });
	setLoader("REMOVE", "EditVNFTransition");
	return rtn;
}
