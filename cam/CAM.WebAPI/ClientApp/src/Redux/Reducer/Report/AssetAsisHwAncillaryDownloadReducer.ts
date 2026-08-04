import {
  AssetAsisHwAncillaryDownload,
  DOWNLOAD_ASSETASISHWANCILLARY,
} from "../../../Model/Report/AssetAsisHwAncillaryExport";

const initState: AssetAsisHwAncillaryDownload = {
  file: null,
};
//const dispatch = useDispatch();

export const AssetAsisHwAncillaryDownloadReducer = (
  state = initState,
  action: { type: string; payload: AssetAsisHwAncillaryDownload }
) => {
  switch (action.type) {
    case DOWNLOAD_ASSETASISHWANCILLARY: {
      return { ...state, file: action.payload.file };
    }
    default:
      return state;
  }
};
