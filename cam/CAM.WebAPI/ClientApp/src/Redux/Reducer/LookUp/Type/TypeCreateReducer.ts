import { TypeCreate } from "../../../../Model/LookUp/Type";

const initState: TypeCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const TypeCreateReducer = (
  state = initState,
  action: { type: string; payload: TypeCreate }
) => {
  switch (action.type) {
    case "CREATE_TYPE": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_TYPE":

      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
