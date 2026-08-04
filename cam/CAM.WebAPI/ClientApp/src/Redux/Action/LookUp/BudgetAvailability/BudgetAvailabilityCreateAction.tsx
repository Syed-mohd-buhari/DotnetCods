import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { BudgetAvailabilityApi } from "../../../../Business/LookUp/BudgetAvailabilityBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, LookUpCreateRule, TipologicaGridDtoCombinationRule } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetBudgetAvailabilityCreateResource() {
	setLoader("ADD", "GetBudgetAvailabilityCreateResource");

	let api = new BudgetAvailabilityApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDtoCombinationRule>>(() => api.budgetAvailabilityGetCreateResource());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreateRule;
	rootStore.dispatch({ type: "GET_CREATE_BUDGET_AVAILABILITY", payload: rtn });
	setLoader("REMOVE", "GetBudgetAvailabilityCreateResource");
}

export async function CreatBudgetAvailability(data: TipologicaGridDtoCombinationRule) {
	setLoader("ADD", "CreatBudgetAvailability");
	let api = new BudgetAvailabilityApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.budgetAvailabilityCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreateRule;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_BUDGET_AVAILABILITY", payload: rtn });
	setLoader("REMOVE", "CreatBudgetAvailability");
	return rtn;
}
