import { CNFPriorityEdit } from "../../../../Model/LookUp/CNFPriority";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFPriorityEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const CNFPriorityEditReducer = (
  state = initState,
  action: { type: string; payload: CNFPriorityEdit }
) => {
  switch (action.type) {
    case "EDIT_CNFPRIORITY": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_CNFPRIORITY":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
