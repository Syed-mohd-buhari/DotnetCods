import { IntraVMTypeEdit } from "../../../../Model/LookUp/IntraVMType";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: IntraVMTypeEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const IntraVMTypeEditReducer = (
  state = initState,
  action: { type: string; payload: IntraVMTypeEdit }
) => {
  switch (action.type) {
    case "EDIT_INTRAVMTYPE": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_INTRAVMTYPE":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
