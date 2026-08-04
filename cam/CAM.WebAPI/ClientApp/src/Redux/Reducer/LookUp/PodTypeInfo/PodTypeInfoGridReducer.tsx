import { PodTypeInfoGrid } from "../../../../Model/LookUp/PodTypeInfo";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: PodTypeInfoGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const PodTypeInfoGridReducer = (
  state = initState,
  action: { type: string; payload: PodTypeInfoGrid }
) => {
  switch (action.type) {
    case "GET_GRID_PODTYPEINFO": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_PODTYPEINFO_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_PODTYPEINFO":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
