import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const NetworkFunctionEditReducer = (
  state = initState,
  action: { type: string; payload: LookUpEdit }
) => {
  switch (action.type) {
    case "EDIT_NETWORK_FUNCTION": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_NETWORK_FUNCTION":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
