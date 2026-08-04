import { TypeEdit } from "../../../../Model/LookUp/Type";

const initState: TypeEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const TypeEditReducer = (
  state = initState,
  action: { type: string; payload: TypeEdit }
) => {
  switch (action.type) {
    case "EDIT_TYPE": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_TYPE":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
