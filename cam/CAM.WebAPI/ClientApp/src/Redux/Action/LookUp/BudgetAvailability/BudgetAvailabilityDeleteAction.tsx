import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { BudgetAvailabilityApi } from "../../../../Business/LookUp/BudgetAvailabilityBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteBudgetAvailability(id: number) {
	setLoader("ADD", "deleteBudgetAvailability");
	let api = new BudgetAvailabilityApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.budgetAvailabilityDelete(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_BUDGET_AVAILABILITY", payload: rtn });
	setLoader("REMOVE", "deleteBudgetAvailability");
	return rtn;
}

export async function DeleteDeepBudgetAvailability(id: number) {
	let api = new BudgetAvailabilityApi();
	setLoader("ADD", "DeleteDeepBudgetAvailability");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.budgetAvailabilityDeleteDeep(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "DELETE_BUDGET_AVAILABILITY", payload: rtn });
	setLoader("REMOVE", "DeleteDeepBudgetAvailability");
	return rtn;
}

export async function GetRelatedRecordsBudgetAvailability(id: number) {
	let api = new BudgetAvailabilityApi();
	setLoader("ADD", "GetRelatedRecordsBudgetAvailability");
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.budgetAvailabilityGetRelatedRecords(id));
	let rtn = { data: result?.data, info: result?.info, warning: result?.warning } as ResultDto;
	if (result?.warning) rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: NotifyType.error }));
	setLoader("REMOVE", "GetRelatedRecordsBudgetAvailability");
	return rtn;
}
