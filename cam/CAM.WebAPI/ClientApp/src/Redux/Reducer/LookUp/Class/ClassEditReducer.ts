import { ClassEdit } from "../../../../Model/LookUp/Class";

const initState: ClassEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const ClassEditReducer = (
  state = initState,
  action: { type: string; payload: ClassEdit }
) => {
  switch (action.type) {
    case "EDIT_CLASS": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_CLASS":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
