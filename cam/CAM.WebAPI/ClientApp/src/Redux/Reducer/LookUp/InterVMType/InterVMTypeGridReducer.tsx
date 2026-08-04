import { InterVMTypeGrid } from "../../../../Model/LookUp/InterVMType";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: InterVMTypeGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const InterVMTypeGridReducer = (
  state = initState,
  action: { type: string; payload: InterVMTypeGrid }
) => {
  switch (action.type) {
    case "GET_GRID_INTERVMTYPE": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_INTERVMTYPE_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_INTERVMTYPE":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
