import {
  GET_GRID_REPORT_HARDWARE_CONFIG,
  GET_FILTER_REPORT_HARDWARE_CONFIG,
  ReportHardwareConfigGrid,
} from "../../../Model/Report/ReportLcmExportModel";

const initState: ReportHardwareConfigGrid = {
  ReportHardwareConfigGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const ReportHardwareConfigGridReducer = (
  state = initState,
  action: { type: string; payload: ReportHardwareConfigGrid }
) => {
  switch (action.type) {
    case GET_GRID_REPORT_HARDWARE_CONFIG: {
      return {
        ...state,
        ReportHardwareConfigGridResult:
          action.payload.ReportHardwareConfigGridResult,
      };
    }
    case GET_FILTER_REPORT_HARDWARE_CONFIG:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
