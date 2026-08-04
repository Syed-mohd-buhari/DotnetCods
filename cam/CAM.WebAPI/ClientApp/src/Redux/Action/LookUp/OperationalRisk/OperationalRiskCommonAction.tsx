import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { OperationalRiskApi } from "../../../../Business/LookUp/OperationalRiskBusiness";
import { ResultDto, ChangeGridOrderDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function ChangeGridOrderOperationalRisk(data: Array<ChangeGridOrderDto>) {
	setLoader("ADD", "ChangeGridOrderOperationalRisk");

	let api = new OperationalRiskApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.operationalRiskChangeGridOrderOperationalRisk(data));
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	setLoader("REMOVE", "ChangeGridOrderOperationalRisk");

	return result;
}
