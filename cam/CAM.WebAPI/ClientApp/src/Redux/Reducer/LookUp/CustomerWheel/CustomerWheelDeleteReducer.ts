import { ResultDto } from "../../../../Model/CommonModels";

const initState: ResultDto = {
  data: undefined,
  info: undefined,
  warning: undefined,
};
//const dispatch = useDispatch();

export const CustomerWheelDeleteReducer = (
  state = initState,
  action: { type: string; payload: ResultDto }
) => {
  switch (action.type) {
    case "DELETE_CUSTOMER_WHEEL": {
      return { ...state, ResultDto: action.payload };
    }
    default:
      return state;
  }
};
