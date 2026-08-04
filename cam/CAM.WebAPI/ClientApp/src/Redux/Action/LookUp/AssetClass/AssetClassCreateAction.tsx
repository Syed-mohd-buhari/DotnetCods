import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { AssetClassApi } from "../../../../Business/LookUp/AssetClassBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  LookUpCreate,
  TipologicaGridDto,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetAssetClassCreateResource() {
  setLoader("ADD", "GetAssetClassCreateResource");

  let api = new AssetClassApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<TipologicaGridDto>
  >(() => api.assetClassGetCreateResourceAssetClass());
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as LookUpCreate;
  rootStore.dispatch({ type: "GET_CREATE_ASSET_CLASS", payload: rtn });
  setLoader("REMOVE", "GetAssetClassCreateResource");
}

export async function CreatAssetClass(data: TipologicaGridDto) {
  setLoader("ADD", "CreatAssetClass");
  let api = new AssetClassApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.assetClassCreate(data)
  );
  let rtn = { ResultDtoCreate: result, LookUpDtoCreate: null } as LookUpCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_ASSET_CLASS", payload: rtn });
  setLoader("REMOVE", "CreatAssetClass");
  return rtn;
}
