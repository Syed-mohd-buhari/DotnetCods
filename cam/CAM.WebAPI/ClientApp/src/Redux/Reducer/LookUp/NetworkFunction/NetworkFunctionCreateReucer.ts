import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};

export const NetworkFunctionCreateReducer = (
  state = initState,
  action: { type: string; payload: LookUpCreate }
) => {
  switch (action.type) {
    case "CREATE_NETWORK_FUNCTION": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_NETWORK_FUNCTION":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
