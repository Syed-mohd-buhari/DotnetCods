import {
  GET_FILTER_BPT_REPORT,
  GET_GRID_BPT_REPORT,
  BPTReportGrid,
} from "../../../Model/BPTReport";

const initialState: BPTReportGrid = {
  BPTReportGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const BPTReportGridReducer = (
  state = initialState,
  action: { type: string; payload: BPTReportGrid }
) => {
  switch (action.type) {
    case GET_GRID_BPT_REPORT: {
      return {
        ...state,
        BPTReportGridResult: action.payload.BPTReportGridResult,
      };
    }
    case GET_FILTER_BPT_REPORT:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
