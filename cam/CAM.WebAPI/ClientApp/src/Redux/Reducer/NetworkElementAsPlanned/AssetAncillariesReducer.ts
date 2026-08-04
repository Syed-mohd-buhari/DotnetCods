import {
  AssetHardwareAncillariesResponse,
  AssetHardwareAncillariesState,
  GET_ASSET_HARDWARE_ANCILLARIES,
} from "../../../Model/NetworkElementAsPlanned";

const initState: AssetHardwareAncillariesState = {
  assetHardwareAncillariesResult: null,
};

export const AssetHardwareAncillariesReducer = (
  state = initState,
  action: {
    type: string;
    payload: AssetHardwareAncillariesState;
  }
) => {
  switch (action.type) {
    case GET_ASSET_HARDWARE_ANCILLARIES:
      return {
        ...state,
        assetHardwareAncillariesResult:
          action.payload.assetHardwareAncillariesResult,
      };

    default:
      return state;
  }
};
