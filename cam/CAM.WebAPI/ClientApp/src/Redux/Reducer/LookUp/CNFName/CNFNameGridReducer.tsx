import { CNFNameGrid } from "../../../../Model/LookUp/CNFName";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFNameGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const CNFNameGridReducer = (
  state = initState,
  action: { type: string; payload: CNFNameGrid }
) => {
  switch (action.type) {
    case "GET_GRID_CNFNAME": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_CNFNAME_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_CNFNAME":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
