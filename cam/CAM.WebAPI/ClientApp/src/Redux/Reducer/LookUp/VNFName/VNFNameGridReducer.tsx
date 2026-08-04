import { VNFNameGrid } from "../../../../Model/LookUp/VNFName";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: VNFNameGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const VNFNameGridReducer = (
  state = initState,
  action: { type: string; payload: VNFNameGrid }
) => {
  switch (action.type) {
    case "GET_GRID_VNFNAME": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_VNFNAME_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_VNFNAME":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
