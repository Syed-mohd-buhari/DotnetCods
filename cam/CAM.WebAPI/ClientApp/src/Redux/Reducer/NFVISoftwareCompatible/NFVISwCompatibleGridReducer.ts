import {
  GET_FILTER_NFVI_COMPATIBLE,
  GET_GRID_NFVI_COMPATIBLE,
  NFVISwCompatibleGrid,
} from "../../../Model/NFVISoftwareCompatible";

const initialState: NFVISwCompatibleGrid = {
  NFVISwCompatibleGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const NFVISwCompatibleGridReducer = (
  state = initialState,
  action: { type: string; payload: NFVISwCompatibleGrid }
) => {
  switch (action.type) {
    case GET_GRID_NFVI_COMPATIBLE: {
      return {
        ...state,
        NFVISwCompatibleGridResult: action.payload.NFVISwCompatibleGridResult,
      };
    }
    case GET_FILTER_NFVI_COMPATIBLE:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
