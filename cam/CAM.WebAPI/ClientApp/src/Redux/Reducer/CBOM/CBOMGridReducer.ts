import { GET_FILTER_CBOM, GET_GRID_CBOM, CBOMGrid } from "../../../Model/CBOM";

const initialState: CBOMGrid = {
  CBOMGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const CBOMGridReducer = (
  state = initialState,
  action: { type: string; payload: CBOMGrid }
) => {
  switch (action.type) {
    case GET_GRID_CBOM: {
      return {
        ...state,
        CBOMGridResult: action.payload.CBOMGridResult,
      };
    }
    case GET_FILTER_CBOM:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
