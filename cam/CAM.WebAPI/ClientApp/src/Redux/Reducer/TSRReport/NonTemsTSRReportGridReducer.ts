import {
  GET_FILTER_NON_TEMS_TSR_REPORT,
  GET_GRID_NON_TEMS_TSR_REPORT,
  NonTemsTSRReportGrid,
} from "../../../Model/TSRReport";

const initialState: NonTemsTSRReportGrid = {
  NonTemsTSRReportGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const NonTemsTSRReportGridReducer = (
  state = initialState,
  action: { type: string; payload: NonTemsTSRReportGrid }
) => {
  switch (action.type) {
    case GET_GRID_NON_TEMS_TSR_REPORT: {
      return {
        ...state,
        NonTemsTSRReportGridResult: action.payload.NonTemsTSRReportGridResult,
      };
    }
    case GET_FILTER_NON_TEMS_TSR_REPORT:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
