import {
  GET_FILTER_NON_TEMS_FNT_REPORT,
  GET_GRID_NON_TEMS_FNT_REPORT,
  NonTemsFNTReportGrid,
} from "../../../Model/FNTReport";

const initialState: NonTemsFNTReportGrid = {
  NonTemsFNTReportGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const NonTemsFNTReportGridReducer = (
  state = initialState,
  action: { type: string; payload: NonTemsFNTReportGrid }
) => {
  switch (action.type) {
    case GET_GRID_NON_TEMS_FNT_REPORT: {
      return {
        ...state,
        NonTemsFNTReportGridResult: action.payload.NonTemsFNTReportGridResult,
      };
    }
    case GET_FILTER_NON_TEMS_FNT_REPORT:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
