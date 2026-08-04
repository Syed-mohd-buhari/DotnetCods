import {
  GET_FILTER_CLUSTER_INFO,
  GET_GRID_CLUSTER_INFO,
  ClusterInfoGrid,
} from "../../../Model/ClusterInfo";

const initialState: ClusterInfoGrid = {
  ClusterInfoGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const ClusterInfoGridReducer = (
  state = initialState,
  action: { type: string; payload: ClusterInfoGrid }
) => {
  switch (action.type) {
    case GET_GRID_CLUSTER_INFO: {
      return {
        ...state,
        ClusterInfoGridResult: action.payload.ClusterInfoGridResult,
      };
    }
    case GET_FILTER_CLUSTER_INFO:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
