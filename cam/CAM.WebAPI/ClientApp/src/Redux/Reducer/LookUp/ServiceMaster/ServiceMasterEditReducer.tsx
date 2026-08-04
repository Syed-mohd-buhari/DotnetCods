import { ServiceMasterEdit } from "../../../../Model/LookUp/ServiceMaster";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: ServiceMasterEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const ServiceMasterEditReducer = (
  state = initState,
  action: { type: string; payload: ServiceMasterEdit }
) => {
  switch (action.type) {
    case "EDIT_SERVICEMASTER": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_SERVICEMASTER":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
