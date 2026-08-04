import { IntraVMTypeGrid } from "../../../../Model/LookUp/IntraVMType";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: IntraVMTypeGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const IntraVMTypeGridReducer = (
  state = initState,
  action: { type: string; payload: IntraVMTypeGrid }
) => {
  switch (action.type) {
    case "GET_GRID_INTRAVMTYPE": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_INTRAVMTYPE_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_INTRAVMTYPE":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
