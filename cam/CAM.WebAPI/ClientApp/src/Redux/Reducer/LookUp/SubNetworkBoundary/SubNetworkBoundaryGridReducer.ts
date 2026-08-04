import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";
import { LookUpSubNetworkBoundaryGrid } from "../../../../Model/LookUp/SubnetworkBoundry";

const initState: LookUpSubNetworkBoundaryGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const SubNetworkBoundaryGridReducer = (
  state = initState,
  action: { type: string; payload: LookUpSubNetworkBoundaryGrid }
) => {
  switch (action.type) {
    case "GET_GRID_SUBNETWORK_BOUNDARY": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_SUBNETWORK_BOUNDARY_ALL": {
      return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult };
    }
    case "GET_FILTER_SUBNETWORK_BOUNDARY":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
