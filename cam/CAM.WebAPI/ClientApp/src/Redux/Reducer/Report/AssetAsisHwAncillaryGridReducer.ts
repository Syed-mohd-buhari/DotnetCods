import {
  GET_GRID_ASSETASISHWANCILLARY,
  GET_FILTER_ASSETASISHWANCILLARY,
  AssetAsisHwAncillaryGrid,
} from "../../../Model/Report/AssetAsisHwAncillaryExport";

const initState: AssetAsisHwAncillaryGrid = {
  AssetAsisHwAncillaryGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const AssetAsisHwAncillaryGridReducer = (
  state = initState,
  action: { type: string; payload: AssetAsisHwAncillaryGrid }
) => {
  switch (action.type) {
    case GET_GRID_ASSETASISHWANCILLARY: {
      return {
        ...state,
        AssetAsisHwAncillaryGridResult:
          action.payload.AssetAsisHwAncillaryGridResult,
      };
    }
    case GET_FILTER_ASSETASISHWANCILLARY:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
