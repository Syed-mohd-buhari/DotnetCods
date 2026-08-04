import {
  GET_GRID_PASSTHROUGH_HARDWARE_REPORT,
  GET_FILTER_PASSTHROUGH_HARDWARE_REPORT,
  PassThroughHardwareReportGrid,
} from "../../../Model/Report/PassThroughHardwareReportExport";

const initState: PassThroughHardwareReportGrid = {
  PassThroughHardwareReportGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const PassThroughHardwareReportGridReducer = (
  state = initState,
  action: { type: string; payload: PassThroughHardwareReportGrid }
) => {
  switch (action.type) {
    case GET_GRID_PASSTHROUGH_HARDWARE_REPORT: {
      return {
        ...state,
        PassThroughHardwareReportGridResult:
          action.payload.PassThroughHardwareReportGridResult,
      };
    }
    case GET_FILTER_PASSTHROUGH_HARDWARE_REPORT:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
