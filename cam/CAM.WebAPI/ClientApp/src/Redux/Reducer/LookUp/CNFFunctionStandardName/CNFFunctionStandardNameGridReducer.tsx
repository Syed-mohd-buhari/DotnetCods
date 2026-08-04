import { CNFFunctionStandardNameGrid } from "../../../../Model/LookUp/CNFFunctionStandardName";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFFunctionStandardNameGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const CNFFunctionStandardNameGridReducer = (
  state = initState,
  action: { type: string; payload: CNFFunctionStandardNameGrid }
) => {
  switch (action.type) {
    case "GET_GRID_CNFFUNCTIONSTANDARDNAME": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_CNFFUNCTIONSTANDARD_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_CNFFUNCTIONSTANDARDNAME":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
