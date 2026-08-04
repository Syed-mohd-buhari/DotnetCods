import {
  GET_FILTER_CBOM_CLUSTER_INFO,
  GET_GRID_CBOM_CLUSTER_INFO,
  CBOMClusterInfoGrid,
} from "../../../Model/CBOM";

const initialState: CBOMClusterInfoGrid = {
  CBOMClusterInfoGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const CBOMClusterInfoGridReducer = (
  state = initialState,
  action: { type: string; payload: CBOMClusterInfoGrid }
) => {
  switch (action.type) {
    case GET_GRID_CBOM_CLUSTER_INFO: {
      return {
        ...state,
        CBOMClusterInfoGridResult: action.payload.CBOMClusterInfoGridResult,
      };
    }
    case GET_FILTER_CBOM_CLUSTER_INFO:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
