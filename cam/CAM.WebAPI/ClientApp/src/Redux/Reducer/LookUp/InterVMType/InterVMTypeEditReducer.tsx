import { InterVMTypeEdit } from "../../../../Model/LookUp/InterVMType";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: InterVMTypeEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const InterVMTypeEditReducer = (
  state = initState,
  action: { type: string; payload: InterVMTypeEdit }
) => {
  switch (action.type) {
    case "EDIT_INTERVMTYPE": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_INTERVMTYPE":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
