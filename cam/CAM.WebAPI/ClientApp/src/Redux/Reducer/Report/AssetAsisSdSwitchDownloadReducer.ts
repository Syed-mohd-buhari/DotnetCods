import {
  AssetAsisSdSwitchDownload,
  DOWNLOAD_ASSETASISSDSWITCH,
} from "../../../Model/Report/AssetAsisSdSwitchExport";

const initState: AssetAsisSdSwitchDownload = {
  file: null,
};
//const dispatch = useDispatch();

export const AssetAsisSdSwitchDownloadReducer = (
  state = initState,
  action: { type: string; payload: AssetAsisSdSwitchDownload }
) => {
  switch (action.type) {
    case DOWNLOAD_ASSETASISSDSWITCH: {
      return { ...state, file: action.payload.file };
    }
    default:
      return state;
  }
};
