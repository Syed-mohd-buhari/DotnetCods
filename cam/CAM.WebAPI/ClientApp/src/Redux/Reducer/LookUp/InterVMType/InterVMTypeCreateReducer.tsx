import { InterVMTypeCreate } from "../../../../Model/LookUp/InterVMType";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: InterVMTypeCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const InterVMTypeCreateReducer = (
  state = initState,
  action: { type: string; payload: InterVMTypeCreate }
) => {
  switch (action.type) {
    case "CREATE_INTERVMTYPE": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_INTERVMTYPE":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
