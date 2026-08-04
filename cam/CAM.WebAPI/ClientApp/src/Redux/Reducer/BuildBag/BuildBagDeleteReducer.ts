import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_BUILD_BAG, RESTORE_BUILD_BAG } from "../../../Model/BuildBag";

const initState: ResultDto = {
  data: undefined,
  info: undefined,
  warning: undefined,
};
//const dispatch = useDispatch();

export const BuildBagDeleteReducer = (
  state = initState,
  action: { type: string; payload: ResultDto }
) => {
  switch (action.type) {
    case DELETE_BUILD_BAG: {
      return { ...state, ResultDto: action.payload };
    }
    case RESTORE_BUILD_BAG: {
      return { ...state, ResultDto: action.payload };
    }
    default:
      return state;
  }
};
