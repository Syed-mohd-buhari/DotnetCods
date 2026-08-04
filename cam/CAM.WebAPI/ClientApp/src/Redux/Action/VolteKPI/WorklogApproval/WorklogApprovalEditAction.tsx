import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { VolteKPIWorklogApi } from "../../../../Business/WorklogApprovalBusiness";
import { VolteKPIApiFetchParamCreator, VolteKPIApi } from "../../../../Business/VolteKPIBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { VolteKPIWorklogDto, WorklogApprovalEdit, EDIT_WORKLOG_APPROVALS, } from "../../../../Model/VolteKpi/WorklogApproval";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";


export async function EditWorklogApproval(data: VolteKPIWorklogDto, forced?: boolean) {
	setLoader("ADD", "EditWorklogApproval");
	let api = new VolteKPIWorklogApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() => api.volteKPIWorklogPut(data, forced));
	let rtn = { ResultDtoEdit: result } as WorklogApprovalEdit;
	rootStore.dispatch(setNotification({ message: result?.info ?? "", notifyType: result?.warning ? NotifyType.error : NotifyType.success }));
	rootStore.dispatch({ type: EDIT_WORKLOG_APPROVALS, payload: rtn });
	setLoader("REMOVE", "EditWorklogApproval");
	return rtn;
}

export async function GetWorklogApprovalDuplicates(data: VolteKPIWorklogDto) {
	setLoader("ADD", "GetWorklogApprovalDuplicates");

	let api = new VolteKPIWorklogApi();
	let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
		api.volteKPIWorklogGetDuplicates(
			data.volteKPIId,
			data.volteKPIType,
			data.volteKPIWorklogId,
		)
	);
	setLoader("REMOVE", "GetWorklogApprovalDuplicates");
	return result?.data ?? false
}