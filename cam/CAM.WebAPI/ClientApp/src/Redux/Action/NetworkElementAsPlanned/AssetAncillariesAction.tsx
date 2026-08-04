import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { NetworkElementAsPlannedApi } from "../../../Business/NetworkElementAsPlannedBusiness";
import { getOpCoAssetsId, getResourceObject } from "../../../Model/Common";
import { ResultDto } from "../../../Model/CommonModels";
import {
  AssetHardwareAncillariesResponse,
  AssetHardwareAncillariesState,
  GET_ASSET_HARDWARE_ANCILLARIES,
} from "../../../Model/NetworkElementAsPlanned";

import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetAssetHardwareAncillaries(obj: getOpCoAssetsId) {
  setLoader("ADD", "GetAssetHardwareAncillaries");

  let api = new NetworkElementAsPlannedApi();
  let result: AssetHardwareAncillariesResponse | null = null;

  try {
    const resultDto = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
      api.AssetHardwareAncillaries(obj)
    );

    result = (resultDto as AssetHardwareAncillariesResponse) ?? null;
    console.log("API result:", result);

    const rtn: AssetHardwareAncillariesState = {
      assetHardwareAncillariesResult: result,
    };

    rootStore.dispatch({
      type: GET_ASSET_HARDWARE_ANCILLARIES,
      payload: rtn,
    });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Failed to fetch asset hardware ancillaries data",
        notifyType: NotifyType.error,
      })
    );

    rootStore.dispatch({
      type: GET_ASSET_HARDWARE_ANCILLARIES,
      payload: {
        assetHardwareAncillariesResult: null,
      },
    });
  }

  setLoader("REMOVE", "GetAssetHardwareAncillaries");
}

export async function SaveAssetHardwareAncillaries(
  body: AssetHardwareAncillariesResponse
) {
  setLoader("ADD", "SaveAssetHardwareAncillaries");
  const api = new NetworkElementAsPlannedApi();

  try {
    const resultDto = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
      api.AddUpdateAsset(body)
    );

    rootStore.dispatch(
      setNotification({
        message: "Saved successfully",
        notifyType: NotifyType.success,
      })
    );

    return resultDto;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Failed to save asset data",
        notifyType: NotifyType.error,
      })
    );
  }

  setLoader("REMOVE", "SaveAssetHardwareAncillaries");
}
