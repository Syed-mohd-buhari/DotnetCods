import { CNFNameCreate } from "../../../../Model/LookUp/CNFName";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFNameCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const CNFNameCreateReducer = (
  state = initState,
  action: { type: string; payload: CNFNameCreate }
) => {
  switch (action.type) {
    case "CREATE_CNFNAME": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_CNFNAME":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
