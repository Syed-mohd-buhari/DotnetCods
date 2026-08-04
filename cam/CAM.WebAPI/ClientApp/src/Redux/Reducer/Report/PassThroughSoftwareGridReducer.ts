import {
  GET_GRID_PASSTHROUGH_SOFTWARE_REPORT,
  GET_FILTER_PASSTHROUGH_SOFTWARE_REPORT,
  PassThroughSoftwareReportGrid,
} from "../../../Model/Report/PassThroughSoftwareReportExport";

const initState: PassThroughSoftwareReportGrid = {
  PassThroughSoftwareReportGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const PassThroughSoftwareReportGridReducer = (
  state = initState,
  action: { type: string; payload: PassThroughSoftwareReportGrid }
) => {
  switch (action.type) {
    case GET_GRID_PASSTHROUGH_SOFTWARE_REPORT: {
      return {
        ...state,
        PassThroughSoftwareReportGridResult:
          action.payload.PassThroughSoftwareReportGridResult,
      };
    }
    case GET_FILTER_PASSTHROUGH_SOFTWARE_REPORT:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
