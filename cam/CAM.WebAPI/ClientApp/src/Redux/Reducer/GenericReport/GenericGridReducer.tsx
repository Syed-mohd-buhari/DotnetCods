import { GET_GRID_AGGREGATED_GENERIC_REPORT, GET_GRID_DISAGGREGATED_GENERIC_REPORT, GET_FILTER_AGGREGATED_GENERIC_REPORT, GET_FILTER_DISAGGREGATED_GENERIC_REPORT, GenericAggregatedReportGrid, GenericDisAggregatedReportGrid } from "../../../Model/GenericReport";
import { GET_GRID_GENERIC_REPORT, GET_GRID_GENERIC_PREVIEW_REPORT, GenericReportGrid, GenericPreviewReportGrid, GET_FILTER_GENERIC_REPORT, GET_FILTER_GENERIC_PREVIEW_REPORT } from "../../../Model/GenericReport";

  
  const aggregatedInitialState: GenericAggregatedReportGrid = {
    GenericAggregatedReportGridResult: null,
    filter: null,
  };

  const disAggregatedInitialState: GenericDisAggregatedReportGrid = {
    GenericDisAggregatedReportGridResult: null,
    filter: null,
  };

  const initState: GenericReportGrid = {
    GenericReportGridResult: null,
    filter: null,
  };
  const initialState: GenericPreviewReportGrid = {
    GenericPreviewReportGridResult: null,
    filter: null,
  };
  //const dispatch = useDispatch();
  
  export const GenericReportGridReducer = (
    state = initState,
    action: { type: string; payload: GenericReportGrid }
  ) => {
    switch (action.type) {
      case GET_GRID_GENERIC_REPORT: {
        return { ...state, GenericReportGridResult: action.payload.GenericReportGridResult };
      }
      case GET_FILTER_GENERIC_REPORT:
        return { ...state, filter: action.payload.filter };
      default:
        return state;
    }
  };

  export const GenericAggregatedReportGridReducer = (
    state = aggregatedInitialState,
    action: { type: string; payload: GenericAggregatedReportGrid }
  ) => {
    switch (action.type) {
      case GET_GRID_AGGREGATED_GENERIC_REPORT: {
        return { ...state, GenericAggregatedReportGridResult: action.payload.GenericAggregatedReportGridResult };
      }
      case GET_FILTER_AGGREGATED_GENERIC_REPORT:
        return { ...state, filter: action.payload.filter };
      default:
        return state;
    }
  };

  export const GenericDisAggregatedReportGridReducer = (
    state = disAggregatedInitialState,
    action: { type: string; payload: GenericDisAggregatedReportGrid }
  ) => {
    switch (action.type) {
      case GET_GRID_DISAGGREGATED_GENERIC_REPORT: {
        return { ...state, GenericDisAggregatedReportGridResult: action.payload.GenericDisAggregatedReportGridResult };
      }
      case GET_FILTER_DISAGGREGATED_GENERIC_REPORT:
        return { ...state, filter: action.payload.filter };
      default:
        return state;
    }
  };

  export const GenericPreviewReportGridReducer = (
    state = initialState,
    action: { type: string; payload: GenericPreviewReportGrid }
  ) => {
    switch (action.type) {
      case GET_GRID_GENERIC_PREVIEW_REPORT: {
        return { ...state, GenericPreviewReportGridResult: action.payload.GenericPreviewReportGridResult };
      }
      case GET_FILTER_GENERIC_PREVIEW_REPORT:
        return { ...state, filter: action.payload.filter };
      default:
        return state;
    }
  };
  