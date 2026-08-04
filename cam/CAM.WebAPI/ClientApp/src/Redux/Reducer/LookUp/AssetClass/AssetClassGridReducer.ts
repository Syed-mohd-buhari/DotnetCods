import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { initState } from "../ActivityStatus/ActivityStatusGridReducer";

export const AssetClassGridReducer = (
  state = initState,
  action: { type: string; payload: LookUpGrid }
) => {
  switch (action.type) {
    case "GET_GRID_ASSET_CLASS": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_ASSET_CLASS_ALL": {
      return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult };
    }
    case "GET_FILTER_ASSET_CLASS":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
