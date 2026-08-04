import { CNFNameEdit } from "../../../../Model/LookUp/CNFName";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFNameEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const CNFNameEditReducer = (
  state = initState,
  action: { type: string; payload: CNFNameEdit }
) => {
  switch (action.type) {
    case "EDIT_CNFNAME": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_CNFNAME":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
