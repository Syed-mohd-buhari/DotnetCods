import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const SupportedServiceCreateReducer = (
  state = initState,
  action: { type: string; payload: LookUpCreate }
) => {
  switch (action.type) {
    case "CREATE_SUPPORTED_SERVICE": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_SUPPORTED_SERVICE":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
