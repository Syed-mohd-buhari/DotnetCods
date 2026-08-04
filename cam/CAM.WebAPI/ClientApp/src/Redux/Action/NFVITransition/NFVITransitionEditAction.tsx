import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NFVITransitionApiFetchParamCreator, NFVITransitionApi } from "../../../Business/NFVITransitionBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { NFVITransitionDtoUpdate, NFVITransitionEdit, EDIT_NFVI_TRAMSITION, GET_EDIT_NFVI_TRAMSITION } from "../../../Model/NFVITransition";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetNFVITransitionEditResource(id: number) {
	setLoader("ADD", "GetNFVITransitionEditResource");

	// const dispach = useDispatch();
	let api = new NFVITransitionApi();
	let createResource = await ApiCallWithErrorHandling<Promise<NFVITransitionDtoUpdate>>(() => api.nFVITransitionGetUpdateResourceVNFTransition(id));
	let rtn = { NFVITransitionDtoEdit: createResource } as NFVITransitionEdit;
	rootStore.dispatch({ type: GET_EDIT_NFVI_TRAMSITION, payload: rtn });
	setLoader("REMOVE", "GetNFVITransitionReport");

	return rtn;
}

export async function EditNFVITransition(data: NFVITransitionDtoUpdate, forced?: boolean) {
	setLoader("ADD", "EditNFVITransition");
	let api = new NFVITransitionApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.nFVITransitionPut(data, forced));
	let rtn = { ResultDtoEdit: result } as NFVITransitionEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: EDIT_NFVI_TRAMSITION, payload: rtn });
	setLoader("REMOVE", "EditNFVITransition");
	return rtn;
}
