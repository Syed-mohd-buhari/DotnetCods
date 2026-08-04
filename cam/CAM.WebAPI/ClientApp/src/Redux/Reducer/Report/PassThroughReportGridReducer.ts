import {
  GET_GRID_PASSTHROUGH_REPORT,
  GET_FILTER_PASSTHROUGH_REPORT,
  PassThroughReportGrid,
} from "../../../Model/Report/PassThroughReportExport";

const initState: PassThroughReportGrid = {
  PassThroughReportGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const PassThroughReportGridReducer = (
  state = initState,
  action: { type: string; payload: PassThroughReportGrid }
) => {
  switch (action.type) {
    case GET_GRID_PASSTHROUGH_REPORT: {
      return {
        ...state,
        PassThroughReportGridResult: action.payload.PassThroughReportGridResult,
      };
    }
    case GET_FILTER_PASSTHROUGH_REPORT:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
