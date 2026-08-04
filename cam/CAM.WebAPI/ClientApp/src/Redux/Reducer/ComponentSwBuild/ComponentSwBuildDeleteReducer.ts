import { ResultDto } from "../../../Model/CommonModels";
import {
  DELETE_COMPONENT_SW_BUILD,
  RESTORE_COMPONENT_SW_BUILD,
} from "../../../Model/ComponentSwBuild";

const initState: ResultDto = {
  data: undefined,
  info: undefined,
  warning: undefined,
};
//const dispatch = useDispatch();

export const ComponentSwBuildDeleteReducer = (
  state = initState,
  action: { type: string; payload: ResultDto }
) => {
  switch (action.type) {
    case DELETE_COMPONENT_SW_BUILD: {
      return { ...state, ResultDto: action.payload };
    }
    case RESTORE_COMPONENT_SW_BUILD: {
      return { ...state, ResultDto: action.payload };
    }
    default:
      return state;
  }
};
