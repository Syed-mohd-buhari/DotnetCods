import {
  PassThroughReportDownload,
  DOWNLOAD_PASSTHROUGHREPORT,
} from "../../../Model/Report/PassThroughReportExport";

const initState: PassThroughReportDownload = {
  file: null,
};
//const dispatch = useDispatch();

export const PassThroughReportDownloadReducer = (
  state = initState,
  action: { type: string; payload: PassThroughReportDownload }
) => {
  switch (action.type) {
    case DOWNLOAD_PASSTHROUGHREPORT: {
      return { ...state, file: action.payload.file };
    }
    default:
      return state;
  }
};
