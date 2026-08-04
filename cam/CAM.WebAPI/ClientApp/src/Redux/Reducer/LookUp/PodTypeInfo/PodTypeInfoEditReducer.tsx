import { PodTypeInfoEdit } from "../../../../Model/LookUp/PodTypeInfo";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: PodTypeInfoEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const PodTypeInfoEditReducer = (
  state = initState,
  action: { type: string; payload: PodTypeInfoEdit }
) => {
  switch (action.type) {
    case "EDIT_PODTYPEINFO": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_PODTYPEINFO":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
