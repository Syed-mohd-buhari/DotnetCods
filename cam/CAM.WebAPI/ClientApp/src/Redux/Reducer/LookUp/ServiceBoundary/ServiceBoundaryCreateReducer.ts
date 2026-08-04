import { LookUpCreateServiceBoundary } from "../../../../Model/LookUp/ServiceBoundary";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpCreateServiceBoundary = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const ServiceBoundaryCreateReducer = (
  state = initState,
  action: { type: string; payload: LookUpCreateServiceBoundary }
) => {
  switch (action.type) {
    case "CREATE_SERVICE_BOUNDARY": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_SERVICE_BOUNDARY":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
