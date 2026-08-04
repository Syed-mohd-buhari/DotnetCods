import { ResultDto } from "../../../Model/CommonModels";
import {
  DELETE_DELIVERY_TRACKING,
  RESTORE_DELIVERY_TRACKING,
} from "../../../Model/DeliveryTracking";

const initState: ResultDto = {
  data: undefined,
  info: undefined,
  warning: undefined,
};
//const dispatch = useDispatch();

export const DeliveryTrackingDeleteReducer = (
  state = initState,
  action: { type: string; payload: ResultDto }
) => {
  switch (action.type) {
    case DELETE_DELIVERY_TRACKING: {
      return { ...state, ResultDto: action.payload };
    }
    case RESTORE_DELIVERY_TRACKING: {
      return { ...state, ResultDto: action.payload };
    }
    default:
      return state;
  }
};
