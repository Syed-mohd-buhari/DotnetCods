import { CNFHardwareTypeEdit } from "../../../../Model/LookUp/CNFHardwareType";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFHardwareTypeEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const CNFHardwareTypeEditReducer = (
  state = initState,
  action: { type: string; payload: CNFHardwareTypeEdit }
) => {
  switch (action.type) {
    case "EDIT_CNFHARDWARETYPE": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_CNFHARDWARETYPE":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
