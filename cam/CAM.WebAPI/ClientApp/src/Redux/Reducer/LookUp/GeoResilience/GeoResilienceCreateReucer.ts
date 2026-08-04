import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const BusinessContinuityMethodCreateReducer = (
  state = initState,
  action: { type: string; payload: LookUpCreate }
) => {
  switch (action.type) {
    case "CREATE_GEO_RESILIENCE": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_GEO_RESILIENCE":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
