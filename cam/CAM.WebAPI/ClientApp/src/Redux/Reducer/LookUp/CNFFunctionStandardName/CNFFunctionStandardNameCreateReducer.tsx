import { CNFFunctionStandardNameCreate } from "../../../../Model/LookUp/CNFFunctionStandardName";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFFunctionStandardNameCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const CNFFunctionStandardNameCreateReducer = (
  state = initState,
  action: { type: string; payload: CNFFunctionStandardNameCreate }
) => {
  switch (action.type) {
    case "CREATE_CNFFUNCTIONSTANDARDNAME": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_CNFFUNCTIONSTANDARDNAME":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
