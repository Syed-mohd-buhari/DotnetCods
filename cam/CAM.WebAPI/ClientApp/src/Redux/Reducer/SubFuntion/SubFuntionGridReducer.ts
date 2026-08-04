import {
  GET_FILTER_SUB_FUNCTION_FILTER,
  GET_GRID_SUB_FUNCTION_FILTER,
  SubFunctionFilterGrid,
} from "../../../Model/SoftwareConfiguration";

const initState: SubFunctionFilterGrid = {
  SubFunctionFilterGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const SubFunctionFilterGridResult = (
  state = initState,
  action: { type: string; payload: SubFunctionFilterGrid }
) => {
  switch (action.type) {
    case GET_GRID_SUB_FUNCTION_FILTER: {
      return {
        ...state,
        SubFunctionFilterGridResult: action.payload.SubFunctionFilterGridResult,
      };
    }
    case GET_FILTER_SUB_FUNCTION_FILTER:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
