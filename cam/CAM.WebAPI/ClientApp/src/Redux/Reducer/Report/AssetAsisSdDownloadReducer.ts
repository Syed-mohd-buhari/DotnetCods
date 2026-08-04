import {
  AssetAsisSdDownload,
  DOWNLOAD_ASSETASISSD,
} from "../../../Model/Report/AssetAsisSdExport";

const initState: AssetAsisSdDownload = {
  file: null,
};
//const dispatch = useDispatch();

export const AssetAsisSdDownloadReducer = (
  state = initState,
  action: { type: string; payload: AssetAsisSdDownload }
) => {
  switch (action.type) {
    case DOWNLOAD_ASSETASISSD: {
      return { ...state, file: action.payload.file };
    }
    default:
      return state;
  }
};
