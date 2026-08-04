import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { OperationalRiskApi } from "../../../../Business/LookUp/OperationalRiskBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { LookUpEditRisk, OperationalRiskDto } from "../../../../Model/LookUp/OperationalRisk";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetOperationalRiskEditResource(id: number) {
	setLoader("ADD", "GetOperationalRiskEditResource");

	let api = new OperationalRiskApi();
	let createResource = await ApiCallWithErrorHandling<Promise<OperationalRiskDto>>(() => api.operationalRiskGetUpdateResourceOperationalRisk(id));
	let rtn = { LookUpDtoEdit: createResource } as LookUpEditRisk;
	rootStore.dispatch({ type: "GET_EDIT_OPERATIONAL_RISK", payload: rtn });
	setLoader("REMOVE", "GetOperationalRiskEditResource");

	return rtn;
}

export async function EditOperationalRisk(data: OperationalRiskDto) {
	setLoader("ADD", "EditOperationalRisk");
	let api = new OperationalRiskApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.operationalRiskPut(data));
	let rtn = { ResultDtoEdit: result } as OperationalRiskDto;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: "EDIT_OPERATIONAL_RISK", payload: rtn });
	setLoader("REMOVE", "EditOperationalRisk");
	return rtn;
}
