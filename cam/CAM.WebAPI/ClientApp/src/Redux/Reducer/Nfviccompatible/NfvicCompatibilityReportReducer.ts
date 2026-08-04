// NfvicCompatibilityReportReducer.ts



  import { NFVIC_COMPATIBILITY_REPORT_REQUEST,NFVIC_COMPATIBILITY_REPORT_SUCCESS,
    NFVIC_COMPATIBILITY_REPORT_FAILURE, 
    NFVICompatibilityReportResponse} from "../../../Model/NfvicCompatible";
  
  
  // Define the state interface for the report
  export interface NFVICCompatibilityReportState {
    loading: boolean;
    error: string | null;
    reportData: NFVICompatibilityReportResponse | null;
  }
  
  const initialState: NFVICCompatibilityReportState = {
    loading: false,
    error: null,
    reportData: null,
  };
  
  type Action = 
    | { type: typeof NFVIC_COMPATIBILITY_REPORT_REQUEST }
    | { type: typeof NFVIC_COMPATIBILITY_REPORT_SUCCESS; payload: NFVICompatibilityReportResponse }
    | { type: typeof NFVIC_COMPATIBILITY_REPORT_FAILURE; payload: string };
  
  export const NFVICCompatibilityReportReducer = (
    state = initialState,
    action: Action
  ): NFVICCompatibilityReportState => {
    switch (action.type) {
      case NFVIC_COMPATIBILITY_REPORT_REQUEST:
        return {
          ...state,
          loading: true,
          error: null,
        };
      case NFVIC_COMPATIBILITY_REPORT_SUCCESS:
        return {
          ...state,
          loading: false,
          reportData: action.payload,
          error: null,
        };
      case NFVIC_COMPATIBILITY_REPORT_FAILURE:
        return {
          ...state,
          loading: false,
          error: action.payload,
        };
      default:
        return state;
    }
  };
  