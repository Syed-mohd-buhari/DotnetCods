import {
  GET_FILTER_VBOM_VNF_INSTANCE_AND_CAPACITY,
  GET_GRID_VBOM_VNF_INSTANCE_AND_CAPACITY,
  VBOMVnfInstanceAndCapacityGrid,
} from "../../../Model/VBOMInfo";

const initialState: VBOMVnfInstanceAndCapacityGrid = {
  VBOMVnfInstanceAndCapacityGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const VBOMClusterInstanceCapacityGridReducer = (
  state = initialState,
  action: { type: string; payload: VBOMVnfInstanceAndCapacityGrid }
) => {
  switch (action.type) {
    case GET_GRID_VBOM_VNF_INSTANCE_AND_CAPACITY: {
      return {
        ...state,
        VBOMVnfInstanceAndCapacityGridResult:
          action.payload.VBOMVnfInstanceAndCapacityGridResult,
      };
    }
    case GET_FILTER_VBOM_VNF_INSTANCE_AND_CAPACITY:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
