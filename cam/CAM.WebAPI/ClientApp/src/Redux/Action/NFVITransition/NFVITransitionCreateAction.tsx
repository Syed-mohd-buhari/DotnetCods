import { type } from "os";
import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NFVITransitionApiFetchParamCreator, NFVITransitionApi } from "../../../Business/NFVITransitionBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { CREATE_NFVI_TRAMSITION, NFVITransitionCreate, NFVITransitionDtoCreate, GET_CREATE_NFVI_TRAMSITION } from "../../../Model/NFVITransition";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetNFVITransitionCreateResource() {
	setLoader("ADD", "GetNFVITransitionCreateResource");

	// const dispach = useDispatch();
	// ActionCenter<>
	// let test = await  ActionCenter<Promise<NFVITransitionDtoCreate>>(() => api.nFVITransitionGetCreateResourceNFVITransition());
	let api = new NFVITransitionApi();

	let createResource = await ApiCallWithErrorHandling<Promise<NFVITransitionDtoCreate>>(() => api.nFVITransitionGetCreateResourceVNFTransition());
	let rtn = { ResultDtoCreate: null, NFVITransitionDtoCreate: createResource } as NFVITransitionCreate;
	rootStore.dispatch({ type: GET_CREATE_NFVI_TRAMSITION, payload: rtn });
	setLoader("REMOVE", "GetNFVITransitionCreateResource");
}

export async function CreatNFVITransition(data: NFVITransitionDtoCreate, forced?: boolean) {
	setLoader("ADD", "CreatNFVITransition");
	let api = new NFVITransitionApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVITransitionCreate(data, forced));
	let rtn = { ResultDtoCreate: result, NFVITransitionDtoCreate: null } as NFVITransitionCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: CREATE_NFVI_TRAMSITION, payload: rtn });
	setLoader("REMOVE", "CreatNFVITransition");
	return rtn;
}
