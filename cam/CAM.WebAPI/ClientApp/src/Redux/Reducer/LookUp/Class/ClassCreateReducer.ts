import { ClassCreate } from "../../../../Model/LookUp/Class";

const initState: ClassCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const ClassCreateReducer = (
  state = initState,
  action: { type: string; payload: ClassCreate }
) => {
  switch (action.type) {
    case "CREATE_CLASS": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_CLASS":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
