import { CNFHardwareTypeCreate } from "../../../../Model/LookUp/CNFHardwareType";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFHardwareTypeCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const CNFHardwareTypeCreateReducer = (
  state = initState,
  action: { type: string; payload: CNFHardwareTypeCreate }
) => {
  switch (action.type) {
    case "CREATE_CNFHARDWARETYPE": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_CNFHARDWARETYPE":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
