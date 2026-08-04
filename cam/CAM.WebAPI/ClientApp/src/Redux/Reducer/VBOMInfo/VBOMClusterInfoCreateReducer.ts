import {
  CREATE_VBOM_CLUSTER_INFO,
  GET_CREATE_VBOM_CLUSTER_INFO,
  VBOMClusterInfoCreate,
} from "../../../Model/VBOMInfo";

const initState: VBOMClusterInfoCreate = {
  ResultDtoCreate: null,
  VBOMClusterInfoDtoCreate: null,
};
//const dispatch = useDispatch();

export const VBOMClusterInfoCreateReducer = (
  state = initState,
  action: { type: string; payload: VBOMClusterInfoCreate }
) => {
  switch (action.type) {
    case CREATE_VBOM_CLUSTER_INFO: {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case GET_CREATE_VBOM_CLUSTER_INFO:
      return {
        ...state,
        VBOMClusterInfoDtoCreate: action.payload.VBOMClusterInfoDtoCreate,
      };
    default:
      return state;
  }
};
