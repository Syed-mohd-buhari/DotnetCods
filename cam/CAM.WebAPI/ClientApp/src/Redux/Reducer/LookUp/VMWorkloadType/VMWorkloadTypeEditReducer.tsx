import { VMWorkloadTypeEdit } from "../../../../Model/LookUp/VMWorkloadType";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: VMWorkloadTypeEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const VMWorkloadTypeEditReducer = (
  state = initState,
  action: { type: string; payload: VMWorkloadTypeEdit }
) => {
  switch (action.type) {
    case "EDIT_VMWORKLOADTYPE": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_VMWORKLOADTYPE":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
