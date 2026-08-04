import { WorklogApprovalGrid, GET_WORKLOG_APPROVALS, GET_FILTER_WORKLOG_APPROVAL } from "../../../Model/VolteKpi/WorklogApproval";

const initState: WorklogApprovalGrid = {
	worklogApprovalGridResult: null,
	filter: null,
};
//const dispatch = useDispatch();

export const WorklogApprovalsGridReducer = (state = initState, action: { type: string; payload: WorklogApprovalGrid }) => {
	switch (action.type) {
		case GET_WORKLOG_APPROVALS: {
			return { ...state, worklogApprovalGridResult: action.payload.worklogApprovalGridResult };
		}
		case GET_FILTER_WORKLOG_APPROVAL:
			return { ...state, filter: action.payload.filter };
		default:
			return state;
	}
};
