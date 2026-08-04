import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { initState } from "../ActivityStatus/ActivityStatusGridReducer";

export const SupportedServiceGridReducer = (
  state = initState,
  action: { type: string; payload: LookUpGrid }
) => {
  switch (action.type) {
    case "GET_GRID_SUPPORTED_SERVICE": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_SUPPORTED_SERVICE_ALL": {
      return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult };
    }
    case "GET_FILTER_SUPPORTED_SERVICE":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
