import {
  EDIT_TEST_INFO,
  GET_EDIT_TEST_INFO,
  TestInfoEdit,
} from "../../../Model/TestInfo";

const initState: TestInfoEdit = {
  TestInfoDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const TestInfoEditReducer = (
  state = initState,
  action: { type: string; payload: TestInfoEdit }
) => {
  switch (action.type) {
    case EDIT_TEST_INFO: {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case GET_EDIT_TEST_INFO:
      return { ...state, TestInfoDtoEdit: action.payload.TestInfoDtoEdit };
    default:
      return state;
  }
};
