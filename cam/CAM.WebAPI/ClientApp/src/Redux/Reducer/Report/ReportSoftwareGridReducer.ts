import {
  GET_GRID_REPORT_SOFTWARE,
  GET_FILTER_REPORT_SOFTWARE,
  ReportSoftwareGrid,
} from "../../../Model/Report/ReportSoftwareModel";

const initState: ReportSoftwareGrid = {
  ReportSoftwareGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const ReportSoftwareGridReducer = (
  state = initState,
  action: { type: string; payload: ReportSoftwareGrid }
) => {
  switch (action.type) {
    case GET_GRID_REPORT_SOFTWARE: {
      return {
        ...state,
        ReportSoftwareGridResult: action.payload.ReportSoftwareGridResult,
      };
    }
    case GET_FILTER_REPORT_SOFTWARE:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
