import { VMTypeNameCreate } from "../../../../Model/LookUp/VMTypeName";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: VMTypeNameCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const VMTypeNameCreateReducer = (
  state = initState,
  action: { type: string; payload: VMTypeNameCreate }
) => {
  switch (action.type) {
    case "CREATE_VMTYPENAME": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_VMTYPENAME":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
