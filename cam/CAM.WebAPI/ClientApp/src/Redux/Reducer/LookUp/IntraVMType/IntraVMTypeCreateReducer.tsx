import { IntraVMTypeCreate } from "../../../../Model/LookUp/IntraVMType";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: IntraVMTypeCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const IntraVMTypeCreateReducer = (
  state = initState,
  action: { type: string; payload: IntraVMTypeCreate }
) => {
  switch (action.type) {
    case "CREATE_INTRAVMTYPE": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_INTRAVMTYPE":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
