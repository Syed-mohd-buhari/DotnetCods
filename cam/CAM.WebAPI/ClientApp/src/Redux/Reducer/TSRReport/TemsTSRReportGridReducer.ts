import {
  GET_FILTER_TEMS_TSR_REPORT,
  GET_GRID_TEMS_TSR_REPORT,
  TemsTSRReportGrid,
} from "../../../Model/TSRReport";

const initialState: TemsTSRReportGrid = {
  TemsTSRReportGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const TemsTSRReportGridReducer = (
  state = initialState,
  action: { type: string; payload: TemsTSRReportGrid }
) => {
  switch (action.type) {
    case GET_GRID_TEMS_TSR_REPORT: {
      return {
        ...state,
        TemsTSRReportGridResult: action.payload.TemsTSRReportGridResult,
      };
    }
    case GET_FILTER_TEMS_TSR_REPORT:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
