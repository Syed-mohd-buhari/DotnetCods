import {
  GET_GRID_EXODUS_REPORT,
  ExodusReportGrid,
  ExodusGrid,
  GET_FILTER_EXODUS_REPORT,
} from "../../../Model/Report/Exodus";

const initState: ExodusReportGrid = {
  ExodusReportGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const ExodusReportGridReducer = (
  state = initState,
  action: { type: string; payload: ExodusReportGrid }
) => {
  switch (action.type) {
    case GET_GRID_EXODUS_REPORT: {
      return {
        ...state,
        ExodusReportGridResult: action.payload.ExodusReportGridResult,
      };
    }
    case GET_FILTER_EXODUS_REPORT:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
