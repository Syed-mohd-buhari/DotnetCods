import {
  GET_FILTER_VBOM_CLUSTER_INFO,
  GET_GRID_VBOM_CLUSTER_INFO,
  VBOMClusterInfoGrid,
} from "../../../Model/VBOMInfo";

const initialState: VBOMClusterInfoGrid = {
  VBOMClusterInfoGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const VBOMClusterInfoGridReducer = (
  state = initialState,
  action: { type: string; payload: VBOMClusterInfoGrid }
) => {
  switch (action.type) {
    case GET_GRID_VBOM_CLUSTER_INFO: {
      return {
        ...state,
        VBOMClusterInfoGridResult: action.payload.VBOMClusterInfoGridResult,
      };
    }
    case GET_FILTER_VBOM_CLUSTER_INFO:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
