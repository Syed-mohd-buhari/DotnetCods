import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { PlanningRiskApi } from "../../../../Business/LookUp/PlanningRiskBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreate, TipologicaGridDto } from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetPlanningRiskCreateResource() {
	setLoader("ADD", "GetPlanningRiskCreateResource");

	let api = new PlanningRiskApi();
	let createResource = await ApiCallWithErrorHandling<Promise<TipologicaGridDto>>(() => api.planningRiskGetCreateResourcePlanningRisk());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreate;
	rootStore.dispatch({ type: "GET_CREATE_PLANNING_RISK", payload: rtn });
	setLoader("REMOVE", "GetPlanningRiskCreateResource");
}

export async function CreatPlanningRisk(data: TipologicaGridDto) {
	setLoader("ADD", "CreatPlanningRisk");
	let api = new PlanningRiskApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.planningRiskCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_PLANNING_RISK", payload: rtn });
	setLoader("REMOVE", "CreatPlanningRisk");
	return rtn;
}
