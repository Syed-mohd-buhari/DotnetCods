import {
  AssetPivotByLocationGrid,
  GET_ASSET_BY_LOCATION,
  GET_FILTER_ASSET_BY_LOCATION,
} from "../../../Model/Report/AssetPivotByLocationModel";
import {
  GET_FILTER_REPORT_PAT,
  GET_GRID_REPORT_PAT,
  ReportPATGrid,
} from "../../../Model/Report/PlannedActivityTrackerModel";

const initState: AssetPivotByLocationGrid = {
  //TODO  controllare se serve davvero e se è corretto così
  AssetPivotByLocationGridResult: null,
  filter: null,
};

export const AssetPivotByLocationGridReducer = (
  state = initState,
  action: { type: string; payload: AssetPivotByLocationGrid }
) => {
  switch (action.type) {
    case GET_ASSET_BY_LOCATION: {
      return {
        ...state,
        AssetPivotByLocationGridResult:
          action.payload.AssetPivotByLocationGridResult,
      };
    }
    case GET_FILTER_ASSET_BY_LOCATION:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
