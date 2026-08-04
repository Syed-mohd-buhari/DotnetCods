import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { OperationalRiskApi } from "../../../../Business/LookUp/OperationalRiskBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpCreateRisk, OperationalRiskDto } from "../../../../Model/LookUp/OperationalRisk";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetOperationalRiskCreateResource() {
	setLoader("ADD", "GetOperationalRiskCreateResource");

	let api = new OperationalRiskApi();
	let createResource = await ApiCallWithErrorHandling<Promise<OperationalRiskDto>>(() => api.operationalRiskGetCreateResourceOperationalRisk());
	let rtn = { ResultDtoCreate: null, LookUpDtoCreate: createResource } as LookUpCreateRisk;
	rootStore.dispatch({ type: "GET_CREATE_OPERATIONAL_RISK", payload: rtn });
	setLoader("REMOVE", "GetOperationalRiskCreateResource");
}

export async function CreatOperationalRisk(data: OperationalRiskDto) {
	setLoader("ADD", "CreatOperationalRisk");
	let api = new OperationalRiskApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.operationalRiskCreate(data));
	let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreateRisk;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "CREATE_OPERATIONAL_RISK", payload: rtn });
	setLoader("REMOVE", "CreatOperationalRisk");
	return rtn;
}
