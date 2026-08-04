import { VNFClusterNameGrid } from "../../../../Model/LookUp/VNFClusterName";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: VNFClusterNameGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const VNFClusterNameGridReducer = (
  state = initState,
  action: { type: string; payload: VNFClusterNameGrid }
) => {
  switch (action.type) {
    case "GET_GRID_VNFCLUSTERNAME": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_VNFCLUSTERNAME_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_VNFCLUSTERNAME":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
