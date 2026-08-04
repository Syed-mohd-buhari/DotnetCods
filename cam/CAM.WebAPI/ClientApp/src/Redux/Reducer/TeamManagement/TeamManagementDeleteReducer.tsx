import { ResultDto } from "../../../Model/CommonModels";

const initState: ResultDto = {
  data: undefined,
  info: undefined,
  warning: undefined,
};

export const TeamManagementDeleteReducer = (
  state = initState,
  action: { type: string; payload: ResultDto }
) => {
  switch (action.type) {
    case "DELETE_TEAMMANAGEMENT": {
      return { ...state, ...action.payload };
    }
    default:
      return state;
  }
};
