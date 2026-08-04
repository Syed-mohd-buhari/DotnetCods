import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CriticalAssetTypeApi } from "../../../../Business/LookUp/CriticalAssetType";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function deleteCriticalAssetType(id: number) {
  setLoader("ADD", "deleteCriticalAssetType");
  let api = new CriticalAssetTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.criticalAssetTypeDelete(id)
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
  rootStore.dispatch({ type: "DELETE_CRITICAL_ASSET_TYPE", payload: rtn });
  setLoader("REMOVE", "deleteCriticalAssetType");
  return rtn;
}

export async function DeleteDeepCriticalAssetType(id: number) {
  let api = new CriticalAssetTypeApi();
  setLoader("ADD", "DeleteDeepCriticalAssetType");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.criticalAssetTypeDeleteDeep(id)
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
  rootStore.dispatch({ type: "DELETE_CRITICAL_ASSET_TYPE", payload: rtn });
  setLoader("REMOVE", "DeleteDeepCriticalAssetType");
  return rtn;
}

export async function GetRelatedRecordsCriticalAssetType(id: number) {
  let api = new CriticalAssetTypeApi();
  setLoader("ADD", "GetRelatedRecordsCriticalAssetType");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.criticalAssetTypeGetRelatedRecords(id)
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
  setLoader("REMOVE", "GetRelatedRecordsCriticalAssetType");
  return rtn;
}
