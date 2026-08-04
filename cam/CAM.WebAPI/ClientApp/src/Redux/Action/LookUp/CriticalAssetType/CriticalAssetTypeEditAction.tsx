import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CriticalAssetTypeApi } from "../../../../Business/LookUp/CriticalAssetType";
import { ResultDto } from "../../../../Model/CommonModels";
import { CriticalAssetTypeDto } from "../../../../Model/LookUp/CriticalAssetType";
import {
  LookUpEdit,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetCriticalAssetTypeEditResource(id: number) {
  setLoader("ADD", "GetCriticalAssetTypeEditResource");

  let api = new CriticalAssetTypeApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<CriticalAssetTypeDto>
  >(() => api.criticalAssetTypeGetUpdateResourceCriticalAssetType(id));
  let rtn = { LookUpDtoEdit: createResource } as LookUpEdit;
  rootStore.dispatch({ type: "GET_EDIT_CRITICAL_ASSET_TYPE", payload: rtn });
  setLoader("REMOVE", "GetCriticalAssetTypeEditResource");

  return rtn;
}

export async function EditCriticalAssetType(data: CriticalAssetTypeDto) {
  setLoader("ADD", "EditCriticalAssetType");
  let api = new CriticalAssetTypeApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.criticalAssetTypePut(data)
  );
  let rtn = { ResultDtoEdit: result } as CriticalAssetTypeDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_CRITICAL_ASSET_TYPE", payload: rtn });
  setLoader("REMOVE", "EditCriticalAssetType");
  return rtn;
}
