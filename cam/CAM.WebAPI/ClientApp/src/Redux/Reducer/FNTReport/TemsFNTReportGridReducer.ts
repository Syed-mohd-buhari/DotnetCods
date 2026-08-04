import {
  GET_FILTER_TEMS_FNT_REPORT,
  GET_GRID_TEMS_FNT_REPORT,
  TemsFNTReportGrid,
} from "../../../Model/FNTReport";

const initialState: TemsFNTReportGrid = {
  TemsFNTReportGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const TemsFNTReportGridReducer = (
  state = initialState,
  action: { type: string; payload: TemsFNTReportGrid }
) => {
  switch (action.type) {
    case GET_GRID_TEMS_FNT_REPORT: {
      return {
        ...state,
        TemsFNTReportGridResult: action.payload.TemsFNTReportGridResult,
      };
    }
    case GET_FILTER_TEMS_FNT_REPORT:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
