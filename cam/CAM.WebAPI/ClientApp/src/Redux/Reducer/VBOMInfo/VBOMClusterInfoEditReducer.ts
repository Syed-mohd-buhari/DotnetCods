import {
  EDIT_VBOM_CLUSTER_INFO,
  GET_EDIT_VBOM_CLUSTER_INFO,
  VBOMClusterInfoEdit,
} from "../../../Model/VBOMInfo";

const initState: VBOMClusterInfoEdit = {
  VBOMClusterInfoDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const VBOMClusterInfoEditReducer = (
  state = initState,
  action: { type: string; payload: VBOMClusterInfoEdit }
) => {
  switch (action.type) {
    case EDIT_VBOM_CLUSTER_INFO: {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case GET_EDIT_VBOM_CLUSTER_INFO:
      return {
        ...state,
        VBOMClusterInfoDtoEdit: action.payload.VBOMClusterInfoDtoEdit,
      };
    default:
      return state;
  }
};
