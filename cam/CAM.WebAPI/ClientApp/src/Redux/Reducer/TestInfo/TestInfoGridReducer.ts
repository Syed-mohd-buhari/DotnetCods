import {
  GET_GRID_TEST_INFO,
  GET_FILTER_TEST_INFO,
  TestInfoGrid,
} from "../../../Model/TestInfo";

const initialState: TestInfoGrid = {
  TestInfoGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const TestInfoGridReducer = (
  state = initialState,
  action: { type: string; payload: TestInfoGrid }
) => {
  switch (action.type) {
    case GET_GRID_TEST_INFO: {
      return {
        ...state,
        TestInfoGridResult: action.payload.TestInfoGridResult,
      };
    }
    case GET_FILTER_TEST_INFO:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
