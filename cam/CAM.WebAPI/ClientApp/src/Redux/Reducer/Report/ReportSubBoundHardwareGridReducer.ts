import {
  GET_GRID_REPORT_SUBBOUND_HARDWARE,
  GET_FILTER_REPORT_SUBBOUND_HARDWARE,
  ReportSubBoundHardwareGrid,
} from "../../../Model/Report/ReportLcmExportModel";

const initState: ReportSubBoundHardwareGrid = {
  ReportSubBoundHardwareGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const ReportSubBoundHardwareGridReducer = (
  state = initState,
  action: { type: string; payload: ReportSubBoundHardwareGrid }
) => {
  switch (action.type) {
    case GET_GRID_REPORT_SUBBOUND_HARDWARE: {
      return {
        ...state,
        ReportSubBoundHardwareGridResult:
          action.payload.ReportSubBoundHardwareGridResult,
      };
    }
    case GET_FILTER_REPORT_SUBBOUND_HARDWARE:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
