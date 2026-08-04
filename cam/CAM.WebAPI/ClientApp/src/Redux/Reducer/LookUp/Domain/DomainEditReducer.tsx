import { LookUpEdit, LookUpForSystemNamesEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpForSystemNamesEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};

//const dispatch = useDispatch();

export const SystemNamesEditReducer = (
  state = initState,
  action: { type: string; payload: LookUpForSystemNamesEdit }
) => {
  switch (action.type) {
    case "EDIT_SYSTEM_NAME": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_SYSTEM_NAME":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
