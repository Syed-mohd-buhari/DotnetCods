import { AssetTypeGrid } from "../../../../Model/LookUp/AssetType";

const initState: AssetTypeGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};
//const dispatch = useDispatch();
//const dispatch = useDispatch();

export const CriticalAssetTypeGridReducer = (
  state = initState,
  action: { type: string; payload: AssetTypeGrid }
) => {
  switch (action.type) {
    case "GET_GRID_CRITICAL_ASSET_TYPE": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_CRITICAL_ASSET_TYPE_ALL": {
      return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult };
    }
    case "GET_FILTER_CRITICAL_ASSET_TYPE":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
