import {
  GET_GRID_ASSETASISSDSWITCH,
  GET_FILTER_ASSETASISSDSWITCH,
  AssetAsisSdSwitchGrid,
} from "../../../Model/Report/AssetAsisSdSwitchExport";

const initState: AssetAsisSdSwitchGrid = {
  AssetAsisSdSwitchGridResult: null,
  filter: null,
};

export const AssetAsisSdSwitchGridReducer = (
  state = initState,
  action: { type: string; payload: AssetAsisSdSwitchGrid }
) => {
  switch (action.type) {
    case GET_GRID_ASSETASISSDSWITCH: {
      return {
        ...state,
        AssetAsisSdSwitchGridResult: action.payload.AssetAsisSdSwitchGridResult,
      };
    }
    case GET_FILTER_ASSETASISSDSWITCH:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
