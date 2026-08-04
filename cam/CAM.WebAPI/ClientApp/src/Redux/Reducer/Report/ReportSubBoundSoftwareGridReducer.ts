import {
  GET_GRID_REPORT_SUBBOUND_SOFTWARE,
  GET_FILTER_REPORT_SUBBOUND_SOFTWARE,
  ReportSubBoundSoftwareGrid,
} from "../../../Model/Report/ReportLcmExportModel";

const initState: ReportSubBoundSoftwareGrid = {
  ReportSubBoundSoftwareGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const ReportSubBoundSoftwareGridReducer = (
  state = initState,
  action: { type: string; payload: ReportSubBoundSoftwareGrid }
) => {
  switch (action.type) {
    case GET_GRID_REPORT_SUBBOUND_SOFTWARE: {
      return {
        ...state,
        ReportSubBoundSoftwareGridResult:
          action.payload.ReportSubBoundSoftwareGridResult,
      };
    }
    case GET_FILTER_REPORT_SUBBOUND_SOFTWARE:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
