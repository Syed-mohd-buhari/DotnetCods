import {
  PassThroughSoftwareReportDownload,
  DOWNLOAD_PASSTHROUGH_SOFTWARE_REPORT,
} from "../../../Model/Report/PassThroughSoftwareReportExport";

const initState: PassThroughSoftwareReportDownload = {
  file: null,
};
//const dispatch = useDispatch();

export const PassThroughSoftwareReportDownloadReducer = (
  state = initState,
  action: { type: string; payload: PassThroughSoftwareReportDownload }
) => {
  switch (action.type) {
    case DOWNLOAD_PASSTHROUGH_SOFTWARE_REPORT: {
      return { ...state, file: action.payload.file };
    }
    default:
      return state;
  }
};
