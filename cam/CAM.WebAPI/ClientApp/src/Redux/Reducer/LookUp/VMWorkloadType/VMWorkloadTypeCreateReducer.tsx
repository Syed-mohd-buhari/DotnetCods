import { VMWorkloadTypeCreate } from "../../../../Model/LookUp/VMWorkloadType";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: VMWorkloadTypeCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const VMWorkloadTypeCreateReducer = (
  state = initState,
  action: { type: string; payload: VMWorkloadTypeCreate }
) => {
  switch (action.type) {
    case "CREATE_VMWORKLOADTYPE": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_VMWORKLOADTYPE":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
