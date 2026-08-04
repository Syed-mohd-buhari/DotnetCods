import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { BudgetAvailabilityApi } from "../../../../Business/LookUp/BudgetAvailabilityBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDtoCombinationRule } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetBudgetAvailabilityEditResource(id: number) {
	setLoader("ADD", "GetBudgetAvailabilityEditResource");

	let api = new BudgetAvailabilityApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDtoCombinationRule>>(() => api.budgetAvailabilityGetUpdateResource(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_BUDGET_AVAILABILITY", payload: rtn });
	setLoader("REMOVE", "GetBudgetAvailabilityEditResource");

	return rtn;
}

export async function EditBudgetAvailability(data: TipologicaGridDtoCombinationRule) {
	setLoader("ADD", "EditBudgetAvailability");
	let api = new BudgetAvailabilityApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.budgetAvailabilityPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDtoCombinationRule;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_BUDGET_AVAILABILITY", payload: rtn });
	setLoader("REMOVE", "EditBudgetAvailability");
	return rtn;
}
