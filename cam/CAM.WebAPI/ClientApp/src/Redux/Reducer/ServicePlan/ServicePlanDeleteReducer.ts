import { ResultDto } from "../../../Model/CommonModels";

const initState: ResultDto = {
  data: undefined,
  info: undefined,
  warning: undefined,
};
//const dispatch = useDispatch();

export const ServicePlanDeleteReducer = (
  state = initState,
  action: { type: string; payload: ResultDto }
) => {
  switch (action.type) {
    case "DELETE_SERVICE_PLAN": {
      return { ...state, ResultDto: action.payload };
    }
    default:
      return state;
  }
};
