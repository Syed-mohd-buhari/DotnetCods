import {
  GET_FILTER_USER_DEFINED_REPORTS_LOGS,
  GET_GRID_USER_DEFINED_REPORTS_LOGS,
  UserDefinedReportsLogsGrid,
} from "../../../Model/UserDefinedReportsLogs";

const initState: UserDefinedReportsLogsGrid = {
  UserDefinedReportsLogsGridResult: null,
  filter: null,
};

//const dispatch = useDispatch();

export const UserDefinedReportsLogsGridReducer = (
  state = initState,
  action: { type: string; payload: UserDefinedReportsLogsGrid }
) => {
  switch (action.type) {
    case GET_GRID_USER_DEFINED_REPORTS_LOGS: {
      return {
        ...state,
        UserDefinedReportsLogsGridResult:
          action.payload.UserDefinedReportsLogsGridResult,
      };
    }
    case GET_FILTER_USER_DEFINED_REPORTS_LOGS:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
