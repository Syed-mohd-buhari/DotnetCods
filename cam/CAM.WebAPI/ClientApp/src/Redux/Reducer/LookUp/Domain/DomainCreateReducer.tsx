import { LookUpCreate, LookUpForSystemNamesCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpForSystemNamesCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};

//const dispatch = useDispatch();

export const SystemNameCreateReducer = (
  state = initState,
  action: { type: string; payload: LookUpForSystemNamesCreate }
) => {
  switch (action.type) {
    case "CREATE_SYSTEM_NAME": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_SYSTEM_NAME":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
