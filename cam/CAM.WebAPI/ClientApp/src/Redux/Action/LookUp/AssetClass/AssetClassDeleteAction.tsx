import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { AssetClassApi } from "../../../../Business/LookUp/AssetClassBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteAssetClass(id: number) {
  setLoader("ADD", "deleteAssetClass");
  let api = new AssetClassApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.assetClassDelete(id)
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
  rootStore.dispatch({ type: "DELETE_ASSET_CLASS", payload: rtn });
  setLoader("REMOVE", "deleteAssetClass");
  return rtn;
}

export async function DeleteDeepAssetClass(id: number) {
  let api = new AssetClassApi();
  setLoader("ADD", "DeleteDeepAssetClass");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.assetClassDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_ASSET_CLASS", payload: rtn });
  setLoader("REMOVE", "DeleteDeepAssetClass");
  return rtn;
}

export async function GetRelatedRecordsAssetClass(id: number) {
  let api = new AssetClassApi();
  setLoader("ADD", "GetRelatedRecordsAssetClass");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.assetClassGetRelatedRecords(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  if (result?.warning)
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: NotifyType.error,
      })
    );
  setLoader("REMOVE", "GetRelatedRecordsAssetClass");
  return rtn;
}
