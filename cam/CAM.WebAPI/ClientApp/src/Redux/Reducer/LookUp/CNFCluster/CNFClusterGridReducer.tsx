import { CNFClusterGrid } from "../../../../Model/LookUp/CNFCluster";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFClusterGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const CNFClusterGridReducer = (
  state = initState,
  action: { type: string; payload: CNFClusterGrid }
) => {
  switch (action.type) {
    case "GET_GRID_CNFCLUSTER": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_CNFCLUSTER_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_CNFCLUSTER":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
