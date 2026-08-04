import { CNFPriorityCreate } from "../../../../Model/LookUp/CNFPriority";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFPriorityCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const CNFPriorityCreateReducer = (
  state = initState,
  action: { type: string; payload: CNFPriorityCreate }
) => {
  switch (action.type) {
    case "CREATE_CNFPRIORITY": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_CNFPRIORITY":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
