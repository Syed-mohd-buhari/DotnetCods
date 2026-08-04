import {
  GET_GRID_ASSET_LEVEL_EXODUS,
  GET_OPCO_DCF_DROPDOWN,
  AssetLevelExodusGrid,
} from "../../../Model/Report/AssetLevelReportForExodus";

const initState: AssetLevelExodusGrid = {
  AssetLevelExodusResult: null,
  OpcoDcfDropdownResult: null,
  filter: null,
};

export const AssetLevelReportForExodusReducer = (
  state = initState,
  action: { type: string; payload: AssetLevelExodusGrid }
) => {
  switch (action.type) {
    case GET_GRID_ASSET_LEVEL_EXODUS:
      return {
        ...state,
        AssetLevelExodusResult: action.payload.AssetLevelExodusResult,
      };
    case GET_OPCO_DCF_DROPDOWN:
      return {
        ...state,
        OpcoDcfDropdownResult: action.payload.OpcoDcfDropdownResult,
      };
    default:
      return state;
  }
};
