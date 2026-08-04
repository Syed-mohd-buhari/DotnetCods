import {
  GET_GRID_CBOM_REPORT,
  CBOMReportGrid,
} from "../../../Model/CBOMReport";

const initialState: CBOMReportGrid = {
  CBOMReportGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const CBOMReportGridReducer = (
  state = initialState,
  action: { type: string; payload: CBOMReportGrid }
) => {
  switch (action.type) {
    case GET_GRID_CBOM_REPORT: {
      return {
        ...state,
        CBOMReportGridResult: action.payload.CBOMReportGridResult,
      };
    }
    default:
      return state;
  }
};
