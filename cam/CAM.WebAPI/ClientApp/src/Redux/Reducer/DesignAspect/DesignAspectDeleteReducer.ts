import { ResultDto } from "../../../Model/CommonModels";
import {
  DELETE_DESIGN_ASPECT,
  RESTORE_DESIGN_ASPECT,
} from "../../../Model/DesignAspects";

const initState: ResultDto = {
  data: undefined,
  info: undefined,
  warning: undefined,
};
//const dispatch = useDispatch();

export const DesignAspectDeleteReducer = (
  state = initState,
  action: { type: string; payload: ResultDto }
) => {
  switch (action.type) {
    case DELETE_DESIGN_ASPECT:
    case RESTORE_DESIGN_ASPECT: {
      return { ...state, ResultDto: action.payload };
    }
    default:
      return state;
  }
};
