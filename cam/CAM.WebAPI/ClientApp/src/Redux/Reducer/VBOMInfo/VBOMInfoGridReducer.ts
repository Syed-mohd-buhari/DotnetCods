import {
  GET_FILTER_VBOM_INFO,
  GET_GRID_VBOM_INFO,
  VBOMInfoGrid,
} from "../../../Model/VBOMInfo";

const initialState: VBOMInfoGrid = {
  VBOMInfoGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const VBOMInfoGridReducer = (
  state = initialState,
  action: { type: string; payload: VBOMInfoGrid }
) => {
  switch (action.type) {
    case GET_GRID_VBOM_INFO: {
      return {
        ...state,
        VBOMInfoGridResult: action.payload.VBOMInfoGridResult,
      };
    }
    case GET_FILTER_VBOM_INFO:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
