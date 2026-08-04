import { CNFHardwareTypeGrid } from "../../../../Model/LookUp/CNFHardwareType";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFHardwareTypeGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const CNFHardwareTypeGridReducer = (
  state = initState,
  action: { type: string; payload: CNFHardwareTypeGrid }
) => {
  switch (action.type) {
    case "GET_GRID_CNFHARDWARETYPE": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_CNFHARDWARETYPE_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_CNFHARDWARETYPE":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
