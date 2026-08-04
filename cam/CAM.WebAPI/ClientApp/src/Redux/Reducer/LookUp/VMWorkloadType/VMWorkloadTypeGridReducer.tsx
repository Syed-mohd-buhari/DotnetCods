import { VMWorkloadTypeGrid } from "../../../../Model/LookUp/VMWorkloadType";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: VMWorkloadTypeGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const VMWorkloadTypeGridReducer = (
  state = initState,
  action: { type: string; payload: VMWorkloadTypeGrid }
) => {
  switch (action.type) {
    case "GET_GRID_VMWORKLOADTYPE": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_VMWORKLOADTYPE_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_VMWORKLOADTYPE":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
