import { PodTypeInfoCreate } from "../../../../Model/LookUp/PodTypeInfo";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: PodTypeInfoCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const PodTypeInfoCreateReducer = (
  state = initState,
  action: { type: string; payload: PodTypeInfoCreate }
) => {
  switch (action.type) {
    case "CREATE_PODTYPEINFO": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_PODTYPEINFO":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
