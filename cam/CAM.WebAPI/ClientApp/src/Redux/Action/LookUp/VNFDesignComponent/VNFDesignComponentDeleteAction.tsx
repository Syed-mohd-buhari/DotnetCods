import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VNFDesignComponentApi } from "../../../../Business/LookUp/VNFDesignComponentBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteVNFDesignComponent(id: number) {
	setLoader("ADD", "deleteVNFDesignComponent");
	let api = new VNFDesignComponentApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.vNFDesignComponentDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_VNF_DESIGN_COMPONENT", payload: rtn });
	setLoader("REMOVE", "deleteVNFDesignComponent");
	return rtn;
}

export async function DeleteDeepVNFDesignComponent(id: number) {
	let api = new VNFDesignComponentApi();
	setLoader("ADD", "DeleteDeepVNFDesignComponent");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.vNFDesignComponentDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_VNF_DESIGN_COMPONENT", payload: rtn });
	setLoader("REMOVE", "DeleteDeepVNFDesignComponent");
	return rtn;
}

export async function GetRelatedRecordsVNFDesignComponent(id: number) {
	let api = new VNFDesignComponentApi();
	setLoader("ADD", "GetRelatedRecordsVNFDesignComponent");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.vNFDesignComponentGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsVNFDesignComponent");
	return rtn;
}
