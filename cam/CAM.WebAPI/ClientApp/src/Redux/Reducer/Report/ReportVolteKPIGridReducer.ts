import {
  GET_GRID_REPORT_VOLTE_KPI,
  ReportVolteKPIGrid,
} from "../../../Model/Report/ReportVolteKPIModel";

const initState: ReportVolteKPIGrid = {
  //TODO  controllare se serve davvero e se è corretto così
  ReportVolteKPIGridResult: null,
  filter: null,
};

export const ReportVolteKPIGridReducer = (
  state = initState,
  action: { type: string; payload: ReportVolteKPIGrid }
) => {
  switch (action.type) {
    case GET_GRID_REPORT_VOLTE_KPI: {
      return {
        ...state,
        ReportVolteKPIGridResult: action.payload.ReportVolteKPIGridResult,
      };
    }
    default:
      return state;
  }
};
