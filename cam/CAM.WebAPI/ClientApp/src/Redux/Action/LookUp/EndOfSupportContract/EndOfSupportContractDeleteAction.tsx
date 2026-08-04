import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { EndOfSupportContractApi } from "../../../../Business/LookUp/EndOfSupportContract";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteEndOfSupportContract(id: number) {
	setLoader("ADD", "deleteEndOfSupportContract");
	let api = new EndOfSupportContractApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.endOfSupportContractDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_END_OF_SUPPORT_CONTRACT", payload: rtn });
	setLoader("REMOVE", "deleteEndOfSupportContract");
	return rtn;
}

export async function DeleteDeepEndOfSupportContract(id: number) {
	let api = new EndOfSupportContractApi();
	setLoader("ADD", "DeleteDeepEndOfSupportContract");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.endOfSupportContractDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_END_OF_SUPPORT_CONTRACT", payload: rtn });
	setLoader("REMOVE", "DeleteDeepEndOfSupportContract");
	return rtn;
}

export async function GetRelatedRecordsEndOfSupportContract(id: number) {
	let api = new EndOfSupportContractApi();
	setLoader("ADD", "GetRelatedRecordsEndOfSupportContract");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.endOfSupportContractGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsEndOfSupportContract");
	return rtn;
}
