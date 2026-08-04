import { ServicePlanGrid } from "../../../Model/ServicePlan";
import { LookUpGrid } from "../../../Model/LookUp/LookUpGenericModel";

const initState: ServicePlanGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const ServicePlanGridReducer = (
  state = initState,
  action: { type: string; payload: ServicePlanGrid }
) => {
  switch (action.type) {
    case "GET_GRID_SERVICEPLAN": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_SERVICEPLAN_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_SERVICEPLAN":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
