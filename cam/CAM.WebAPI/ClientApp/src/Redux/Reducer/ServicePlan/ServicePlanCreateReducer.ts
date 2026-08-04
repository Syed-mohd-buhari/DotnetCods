import {
  CREATE_SERVICE_PLAN,
  GET_CREATE_SERVICE_PLAN,
  ServicePlanCreate,
} from "../../../Model/ServicePlan";

const initState: ServicePlanCreate = {
  ResultDtoCreate: null,
  ServicePlanDtoCreate: null,
};
//const dispatch = useDispatch();

export const ServicePlanCreateReducer = (
  state = initState,
  action: { type: string; payload: ServicePlanCreate }
) => {
  switch (action.type) {
    case CREATE_SERVICE_PLAN: {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case GET_CREATE_SERVICE_PLAN:
      return {
        ...state,
        ServicePlanDtoCreate: action.payload.ServicePlanDtoCreate,
      };
    default:
      return state;
  }
};
