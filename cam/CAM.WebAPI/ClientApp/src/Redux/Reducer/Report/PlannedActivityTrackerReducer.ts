import {
  GET_FILTER_REPORT_PAT,
  GET_GRID_REPORT_PAT,
  ReportPATGrid,
} from "../../../Model/Report/PlannedActivityTrackerModel";

const initState: ReportPATGrid = {
  //TODO  controllare se serve davvero e se è corretto così
  ReportPATGridResult: null,
  filter: null,
};

export const ReportPATGridReducer = (
  state = initState,
  action: { type: string; payload: ReportPATGrid }
) => {
  switch (action.type) {
    case GET_GRID_REPORT_PAT: {
      return {
        ...state,
        ReportPATGridResult: action.payload.ReportPATGridResult,
      };
    }
    case GET_FILTER_REPORT_PAT:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
