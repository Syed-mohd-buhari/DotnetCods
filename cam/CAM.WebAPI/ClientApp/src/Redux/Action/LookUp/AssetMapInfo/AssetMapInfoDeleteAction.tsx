import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { AssetMapInfoApi } from "../../../../Business/LookUp/AssetMapInfoBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteAssetMapInfo(id: number) {
  setLoader("ADD", "DeleteAssetMapInfo");
  let api = new AssetMapInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.AssetMapInfoDelete(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "DELETE_ASSETMAPINFO", payload: rtn });
  setLoader("REMOVE", "DeleteAssetMapInfo");
  return rtn;
}

export async function DeleteDeepAssetMapInfo(id: number) {
  let api = new AssetMapInfoApi();
  setLoader("ADD", "DeleteDeepAssetMapInfo");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.AssetMapInfoDeleteDeep(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "DELETE_ASSETMAPINFO", payload: rtn });
  setLoader("REMOVE", "DeleteDeepAssetMapInfo");
  return rtn;
}
