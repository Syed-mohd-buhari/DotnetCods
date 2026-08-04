import { CNFFunctionStandardNameEdit } from "../../../../Model/LookUp/CNFFunctionStandardName";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFFunctionStandardNameEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const CNFFunctionStandardNameEditReducer = (
  state = initState,
  action: { type: string; payload: CNFFunctionStandardNameEdit }
) => {
  switch (action.type) {
    case "EDIT_CNFFUNCTIONSTANDARDNAME": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_CNFFUNCTIONSTANDARDNAME":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
