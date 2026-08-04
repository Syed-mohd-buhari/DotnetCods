import { ServiceMasterCreate } from "../../../../Model/LookUp/ServiceMaster";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: ServiceMasterCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const ServiceMasterCreateReducer = (
  state = initState,
  action: { type: string; payload: ServiceMasterCreate }
) => {
  switch (action.type) {
    case "CREATE_SERVICEMASTER": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_SERVICEMASTR":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
