import { ResultDto } from "../../../Model/CommonModels";
import {
  DELETE_ORGANIZATION_INFO,
  RESTORE_ORGANIZATION_INFO,
} from "../../../Model/OrganizationInfo";

const initState: ResultDto = {
  data: undefined,
  info: undefined,
  warning: undefined,
};
//const dispatch = useDispatch();

export const OrganizationInfoDeleteReducer = (
  state = initState,
  action: { type: string; payload: ResultDto }
) => {
  switch (action.type) {
    case DELETE_ORGANIZATION_INFO:
    case RESTORE_ORGANIZATION_INFO: {
      return { ...state, ResultDto: action.payload };
    }
    default:
      return state;
  }
};
