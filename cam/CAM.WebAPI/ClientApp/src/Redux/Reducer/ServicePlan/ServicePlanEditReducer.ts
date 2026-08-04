import {
  EDIT_SERVICE_PLAN,
  GET_EDIT_SERVICE_PLAN,
  ServicePlanEdit,
} from "../../../Model/ServicePlan";

const initState: ServicePlanEdit = {
  ResultDtoEdit: null,
  ServicePlanDtoEdit: null,
};
//const dispatch = useDispatch();

export const ServicePlanEditReducer = (
  state = initState,
  action: { type: string; payload: ServicePlanEdit }
) => {
  switch (action.type) {
    case EDIT_SERVICE_PLAN: {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case GET_EDIT_SERVICE_PLAN:
      return {
        ...state,
        ServicePlanDtoEdit: action.payload.ServicePlanDtoEdit,
      };
    default:
      return state;
  }
};
