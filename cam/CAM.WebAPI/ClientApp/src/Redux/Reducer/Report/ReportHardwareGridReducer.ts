import {
  GET_GRID_REPORT_HARDWARE,
  GET_FILTER_REPORT_HARDWARE,
  ReportHardwareGrid,
} from "../../../Model/Report/ReportHardwareModel";

const initState: ReportHardwareGrid = {
  ReportHardwareGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const ReportHardwareGridReducer = (
  state = initState,
  action: { type: string; payload: ReportHardwareGrid }
) => {
  switch (action.type) {
    case GET_GRID_REPORT_HARDWARE: {
      return {
        ...state,
        ReportHardwareGridResult: action.payload.ReportHardwareGridResult,
      };
    }
    case GET_FILTER_REPORT_HARDWARE:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
