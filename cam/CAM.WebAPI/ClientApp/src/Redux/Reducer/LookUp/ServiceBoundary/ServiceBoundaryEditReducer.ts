import { LookUpEditServiceBoundary } from "../../../../Model/LookUp/ServiceBoundary";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpEditServiceBoundary = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const ServiceBoundaryEditReducer = (
  state = initState,
  action: { type: string; payload: LookUpEditServiceBoundary }
) => {
  switch (action.type) {
    case "EDIT_SERVICE_BOUNDARY": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_SERVICE_BOUNDARY":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
