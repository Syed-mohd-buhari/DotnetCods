import {
  GET_GRID_VBOM_REPORT,
  VBOMReportGrid,
} from "../../../Model/VBOMReport";

const initialState: VBOMReportGrid = {
  VBOMReportGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const VBOMReportGridReducer = (
  state = initialState,
  action: { type: string; payload: VBOMReportGrid }
) => {
  switch (action.type) {
    case GET_GRID_VBOM_REPORT: {
      return {
        ...state,
        VBOMReportGridResult: action.payload.VBOMReportGridResult,
      };
    }
    default:
      return state;
  }
};
