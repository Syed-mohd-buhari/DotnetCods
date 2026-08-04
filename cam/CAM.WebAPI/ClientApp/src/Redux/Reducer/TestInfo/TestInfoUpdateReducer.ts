import {
  CREATE_TEST_INFO,
  GET_CREATE_TEST_INFO,
  TestInfoCreate,
} from "../../../Model/TestInfo";

const initState: TestInfoCreate = {
  ResultDtoCreate: null,
  TestInfoDtoCreate: null,
};
//const dispatch = useDispatch();

export const TestInfoUpdateReducer = (
  state = initState,
  action: { type: string; payload: TestInfoCreate }
) => {
  switch (action.type) {
    case CREATE_TEST_INFO: {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case GET_CREATE_TEST_INFO:
      return { ...state, TestInfoDtoCreate: action.payload.TestInfoDtoCreate };
    default:
      return state;
  }
};
