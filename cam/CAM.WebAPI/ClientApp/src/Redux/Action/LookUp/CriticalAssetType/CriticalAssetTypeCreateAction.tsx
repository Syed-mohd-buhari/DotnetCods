import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { CriticalAssetTypeApi } from "../../../../Business/LookUp/CriticalAssetType";
import { ResultDto } from "../../../../Model/CommonModels";
import { CriticalAssetTypeDto } from "../../../../Model/LookUp/CriticalAssetType";
import {
  LookUpCreate,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetCriticalAssetTypeCreateResource() {
  setLoader("ADD", "GetCriticalAssetTypeCreateResource");

  let api = new CriticalAssetTypeApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<CriticalAssetTypeDto>
  >(() => api.criticalAssetTypeGetCreateResourceCriticalAssetType());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({ type: "GET_CREATE_CRITICAL_ASSET_TYPE", payload: rtn });
  setLoader("REMOVE", "GetCriticalAssetTypeCreateResource");
}

export async function CreatCriticalAssetType(data: CriticalAssetTypeDto) {
  let api = new CriticalAssetTypeApi();
  setLoader("ADD", "CreatCriticalAssetType");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.criticalAssetTypeCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_CRITICAL_ASSET_TYPE", payload: rtn });
  setLoader("REMOVE", "CreatCriticalAssetType");
  return rtn;
}
