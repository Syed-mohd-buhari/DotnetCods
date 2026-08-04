import { VMTypeNameEdit } from "../../../../Model/LookUp/VMTypeName";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: VMTypeNameEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const VMTypeNameEditReducer = (
  state = initState,
  action: { type: string; payload: VMTypeNameEdit }
) => {
  switch (action.type) {
    case "EDIT_VMTYPENAME": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_VMTYPENAME":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
