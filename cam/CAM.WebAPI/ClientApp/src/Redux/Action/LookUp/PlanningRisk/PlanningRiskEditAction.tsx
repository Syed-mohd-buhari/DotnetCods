import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlanningRiskApi } from "../../../../Business/LookUp/PlanningRiskBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEdit, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetPlanningRiskEditResource(id: number) {
	setLoader("ADD", "GetPlanningRiskEditResource");

	let api = new PlanningRiskApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.planningRiskGetUpdateResourcePlanningRisk(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
	rootStore.dispatch({ type: "GET_EDIT_PLANNING_RISK", payload: rtn });
	setLoader("REMOVE", "GetPlanningRiskEditResource");

	return rtn;
}

export async function EditPlanningRisk(data: TipologicaGridDto) {
	setLoader("ADD", "EditPlanningRisk");
	let api = new PlanningRiskApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.planningRiskPut(data));
	let rtn = { ResultDtoEdit: result } as TipologicaGridDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_PLANNING_RISK", payload: rtn });
	setLoader("REMOVE", "EditPlanningRisk");
	return rtn;
}
