import { QueryResultDtoOfVolteKPIWorklogDto, VolteKPIWorklogDto, WorklogApprovalGrid, GET_WORKLOG_APPROVALS, GET_FILTER_WORKLOG_APPROVAL } from '../../../../Model/VolteKpi/WorklogApproval';
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { VolteKPIWorklogApi } from "../../../../Business/WorklogApprovalBusiness";
import { WorklogApprovalQueryObjectGrid } from "../../../../Model/VolteKpi/WorklogApproval";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetWorklogApprovalGrid(queryFilter?: WorklogApprovalQueryObjectGrid) {
	setLoader("ADD", "GetWorklogApprovalGrid");

	let result: QueryResultDtoOfVolteKPIWorklogDto | null | undefined;
	let api = new VolteKPIWorklogApi();

	try {
		result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfVolteKPIWorklogDto>>(() =>
			api.volteKPIWorklogGet(
				queryFilter??{}
			)
		);
		let rtn = { worklogApprovalGridResult: result, filter: null } as WorklogApprovalGrid;
		rootStore.dispatch({ type: GET_WORKLOG_APPROVALS, payload: rtn as WorklogApprovalGrid });
	} catch (error) {
		rootStore.dispatch({ type: GET_WORKLOG_APPROVALS, payload: { worklogApprovalGridResult: result, filter: null } as WorklogApprovalGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	setLoader("REMOVE", "GetWorklogApprovalGrid");
}

export async function GetFilterColumWorklogApproval(columName: string, columValue: string, queryFilter?: WorklogApprovalQueryObjectGrid) {

	let result: FilterValueDto[] | null | undefined;
	let api = new VolteKPIWorklogApi();

	try {
			result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
				api.volteKPIWorklogGetFilterResult(
					queryFilter ?? {}, columName,
					columValue,
				)
			);
		let rtn = { worklogApprovalGridResult: null, filter: result } as WorklogApprovalGrid;
		rootStore.dispatch({ type: GET_FILTER_WORKLOG_APPROVAL, payload: rtn as WorklogApprovalGrid });
	} catch (error) {
		rootStore.dispatch({ type: GET_FILTER_WORKLOG_APPROVAL, payload: { worklogApprovalGridResult: null, filter: result } as WorklogApprovalGrid });
		rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
	}
	// setLoader("REMOVE", "GetFilterColumWorklogApproval");
}
