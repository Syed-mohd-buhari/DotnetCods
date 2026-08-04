import { VNFHardwareTypeGrid } from "../../../../Model/LookUp/VNFHardwareType";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: VNFHardwareTypeGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const VNFHardwareTypeGridReducer = (
  state = initState,
  action: { type: string; payload: VNFHardwareTypeGrid }
) => {
  switch (action.type) {
    case "GET_GRID_VNFHARDWARETYPE": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_VNFHARDWARETYPE_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_VNFHARDWARETYPE":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
