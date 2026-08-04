import { ResultDto } from "../../../Model/CommonModels";
import { DELETE_TEST_INFO, RESTORE_TEST_INFO } from "../../../Model/TestInfo";

const initState: ResultDto = {
  data: undefined,
  info: undefined,
  warning: undefined,
};
//const dispatch = useDispatch();

export const TestInfoDeleteReducer = (
  state = initState,
  action: { type: string; payload: ResultDto }
) => {
  switch (action.type) {
    case DELETE_TEST_INFO:
    case RESTORE_TEST_INFO: {
      return { ...state, ResultDto: action.payload };
    }
    default:
      return state;
  }
};
