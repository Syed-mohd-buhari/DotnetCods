import { ServiceMasterGrid } from "../../../../Model/LookUp/ServiceMaster";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: ServiceMasterGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const ServiceMasterGridReducer = (
  state = initState,
  action: { type: string; payload: ServiceMasterGrid }
) => {
  switch (action.type) {
    case "GET_GRID_SERVICEMASTER": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_SERVICEMASTER_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_SERVICEMASTER":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
