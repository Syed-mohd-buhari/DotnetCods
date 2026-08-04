import {
  GET_FILTER_VBOM_VNF_CAPACITY,
  GET_GRID_VBOM_VNF_CAPACITY,
  VBOMVnfCapacityGrid,
} from "../../../Model/VBOMInfo";

const initialState: VBOMVnfCapacityGrid = {
  VBOMVnfCapacityGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const VBOMClusterCapacityGridReducer = (
  state = initialState,
  action: { type: string; payload: VBOMVnfCapacityGrid }
) => {
  switch (action.type) {
    case GET_GRID_VBOM_VNF_CAPACITY: {
      return {
        ...state,
        VBOMVnfCapacityGridResult: action.payload.VBOMVnfCapacityGridResult,
      };
    }
    case GET_FILTER_VBOM_VNF_CAPACITY:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
