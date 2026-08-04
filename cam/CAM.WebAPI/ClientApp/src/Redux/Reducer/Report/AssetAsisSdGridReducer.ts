import {
  GET_GRID_ASSETASISSD,
  GET_FILTER_ASSETASISSD,
  AssetAsisSdGrid,
} from "../../../Model/Report/AssetAsisSdExport";

const initState: AssetAsisSdGrid = {
  AssetAsisSdGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const AssetAsisSdGridReducer = (
  state = initState,
  action: { type: string; payload: AssetAsisSdGrid }
) => {
  switch (action.type) {
    case GET_GRID_ASSETASISSD: {
      return {
        ...state,
        AssetAsisSdGridResult: action.payload.AssetAsisSdGridResult,
      };
    }
    case GET_FILTER_ASSETASISSD:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
