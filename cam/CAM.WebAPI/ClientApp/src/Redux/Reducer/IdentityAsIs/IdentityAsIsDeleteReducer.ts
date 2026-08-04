import { ResultDto } from "../../../Model/CommonModels";
import {
  DELETE_IDENTiTYASIS,
  RESTORE_IDENTiTYASIS,
} from "../../../Model/LookUp/Identities";

const initState: ResultDto = {
  data: undefined,
  info: undefined,
  warning: undefined,
};
//const dispatch = useDispatch();

export const IdentityAsIsDeleteReducer = (
  state = initState,
  action: { type: string; payload: ResultDto }
) => {
  switch (action.type) {
    case DELETE_IDENTiTYASIS: {
      return { ...state, ResultDto: action.payload };
    }
    case RESTORE_IDENTiTYASIS: {
      return { ...state, ResultDto: action.payload };
    }
    default:
      return state;
  }
};
