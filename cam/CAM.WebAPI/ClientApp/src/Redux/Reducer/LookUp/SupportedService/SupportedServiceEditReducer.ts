import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const SupportedServiceEditReducer = (
  state = initState,
  action: { type: string; payload: LookUpEdit }
) => {
  switch (action.type) {
    case "EDIT_SUPPORTED_SERVICE": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_SUPPORTED_SERVICE":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
