import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { LookUpServiceBoundaryGrid } from "../../../../Model/LookUp/ServiceBoundary";

const initState: LookUpServiceBoundaryGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const ServiceBoundaryGridReducer = (
  state = initState,
  action: { type: string; payload: LookUpServiceBoundaryGrid }
) => {
  switch (action.type) {
    case "GET_GRID_SERVICE_BOUNDARY": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_SERVICE_BOUNDARY_ALL": {
      return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult };
    }
    case "GET_FILTER_SERVICE_BOUNDARY":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
