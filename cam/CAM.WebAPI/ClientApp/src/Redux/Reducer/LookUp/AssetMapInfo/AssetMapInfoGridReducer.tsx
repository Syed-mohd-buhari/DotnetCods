import { AssetMapInfoGrid } from "../../../../Model/LookUp/AssetMapInfo";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: AssetMapInfoGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const AssetMapInfoGridReducer = (
  state = initState,
  action: { type: string; payload: AssetMapInfoGrid }
) => {
  switch (action.type) {
    case "GET_GRID_ASSETMAPINFO": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_ASSETMAPINFO_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_ASSETMAPINFO":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
