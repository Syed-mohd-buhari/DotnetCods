import {
  GET_GRID_REPORT_NETWORKLEVEL2,
  GET_FILTER_REPORT_NETWORKLEVEL2,
  ReportNetworkLevel2Grid,
} from "../../../Model/Report/ReportLcmExportModel";

const initState: ReportNetworkLevel2Grid = {
  ReportNetworkLevel2GridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const ReportNetworkLevel2GridReducer = (
  state = initState,
  action: { type: string; payload: ReportNetworkLevel2Grid }
) => {
  switch (action.type) {
    case GET_GRID_REPORT_NETWORKLEVEL2: {
      return {
        ...state,
        ReportNetworkLevel2GridResult:
          action.payload.ReportNetworkLevel2GridResult,
      };
    }
    case GET_FILTER_REPORT_NETWORKLEVEL2:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
