import { WorklogApprovalEdit, EDIT_WORKLOG_APPROVALS } from '../../../Model/VolteKpi/WorklogApproval';

const initState: WorklogApprovalEdit = {
	ResultDtoEdit: null,
	worklogApprovalDtoEdit: null
};

export const WorklogApprovalsEditReducer = (state = initState, action: { type: string; payload: WorklogApprovalEdit }) => {
	switch (action.type) {
		case EDIT_WORKLOG_APPROVALS: {
			return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
		}
		default:
			return state;
	}
};

