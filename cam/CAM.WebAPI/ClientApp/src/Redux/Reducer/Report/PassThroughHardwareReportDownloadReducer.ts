import {
  PassThroughHardwareReportDownload,
  DOWNLOAD_PASSTHROUGH_HARDWARE_REPORT,
} from "../../../Model/Report/PassThroughHardwareReportExport";

const initState: PassThroughHardwareReportDownload = {
  file: null,
};
//const dispatch = useDispatch();

export const PassThroughHardwareReportDownloadReducer = (
  state = initState,
  action: { type: string; payload: PassThroughHardwareReportDownload }
) => {
  switch (action.type) {
    case DOWNLOAD_PASSTHROUGH_HARDWARE_REPORT: {
      return { ...state, file: action.payload.file };
    }
    default:
      return state;
  }
};
