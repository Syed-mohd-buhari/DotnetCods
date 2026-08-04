import {
  GET_GRID_FEEDBACK_LOGS,
  FeedbackLogsGrid,
  GET_FILTER_FEEDBACK_LOGS,
} from "../../../Model/FeedbackLogs";

const initState: FeedbackLogsGrid = {
  FeedbackLogsGridResult: null,
  filter: null,
};

//const dispatch = useDispatch();

export const FeedbackLogsGridReducer = (
  state = initState,
  action: { type: string; payload: FeedbackLogsGrid }
) => {
  switch (action.type) {
    case GET_GRID_FEEDBACK_LOGS: {
      return {
        ...state,
        FeedbackLogsGridResult: action.payload.FeedbackLogsGridResult,
      };
    }
    case GET_FILTER_FEEDBACK_LOGS:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
