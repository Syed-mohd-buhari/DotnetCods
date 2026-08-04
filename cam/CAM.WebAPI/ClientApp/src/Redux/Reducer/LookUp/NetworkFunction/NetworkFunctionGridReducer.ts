import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { initState } from "../ActivityStatus/ActivityStatusGridReducer";

export const NetworkFunctionGridReducer = (
  state = initState,
  action: { type: string; payload: LookUpGrid }
) => {
  switch (action.type) {
    case "GET_GRID_NETWORK_FUNCTION": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_NETWORK_FUNCTION_ALL": {
      return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult };
    }
    case "GET_FILTER_NETWORK_FUNCTION":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
