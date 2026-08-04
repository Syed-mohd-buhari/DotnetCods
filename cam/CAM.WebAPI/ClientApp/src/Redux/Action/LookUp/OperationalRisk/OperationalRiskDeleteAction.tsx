import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { OperationalRiskApi } from "../../../../Business/LookUp/OperationalRiskBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteOperationalRisk(id: number) {
	setLoader("ADD", "deleteOperationalRisk");
	let api = new OperationalRiskApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.operationalRiskDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_OPERATIONAL_RISK", payload: rtn });
	setLoader("REMOVE", "deleteOperationalRisk");
	return rtn;
}

export async function DeleteDeepOperationalRisk(id: number) {
	let api = new OperationalRiskApi();
	setLoader("ADD", "DeleteDeepOperationalRisk");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.operationalRiskDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_OPERATIONAL_RISK", payload: rtn });
	setLoader("REMOVE", "DeleteDeepOperationalRisk");
	return rtn;
}

export async function GetRelatedRecordsOperationalRisk(id: number) {
	let api = new OperationalRiskApi();
	setLoader("ADD", "GetRelatedRecordsOperationalRisk");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.operationalRiskGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsOperationalRisk");
	return rtn;
}
