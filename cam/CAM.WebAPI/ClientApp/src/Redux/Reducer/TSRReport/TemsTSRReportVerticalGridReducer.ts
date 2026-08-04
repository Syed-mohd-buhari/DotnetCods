import { LookUpGrid } from "../../../Model/LookUp/LookUpGenericModel";
import { GET_GRID_VERTICAL_TEMS_TSR_REPORT } from "../../../Model/TSRReport";
import { initState } from "../LookUp/ActivityStatus/ActivityStatusGridReducer";

export const TSRReportVerticalGridReducer = (
  state = initState,
  action: { type: string; payload: LookUpGrid }
) => {
  switch (action.type) {
    case "GET_GRID_TSR_VERTICAL": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }

    case "GET_FILTER_PRODUCT_IMPORTANCE":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
