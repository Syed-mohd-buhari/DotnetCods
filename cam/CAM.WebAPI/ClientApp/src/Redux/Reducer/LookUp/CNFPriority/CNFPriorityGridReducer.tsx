import { CNFPriorityGrid } from "../../../../Model/LookUp/CNFPriority";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFPriorityGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const CNFPriorityGridReducer = (
  state = initState,
  action: { type: string; payload: CNFPriorityGrid }
) => {
  switch (action.type) {
    case "GET_GRID_CNFPRIORITY": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_CNFPRIORITY_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_CNFPRIORITY":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
