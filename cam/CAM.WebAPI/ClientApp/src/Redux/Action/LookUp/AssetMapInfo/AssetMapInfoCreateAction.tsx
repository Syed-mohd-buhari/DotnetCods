import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { AssetMapInfoApi } from "../../../../Business/LookUp/AssetMapInfoBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'
import {
  AssetMapInfoCreate,
  AssetMapInfoDto,
} from "../../../../Model/LookUp/AssetMapInfo";

export async function GetAssetMapInfoCreateResource() {
  setLoader("ADD", "GetAssetMapInfoCreateResource");

  let api = new AssetMapInfoApi();
  let createResource = await ApiCallWithErrorHandling<Promise<AssetMapInfoDto>>(
    () => api.AssetMapInfoGetCreatepage()
  );
  let rtn = {
    ResultDtoCreate: null,
    LookUpDtoCreate: createResource,
  } as AssetMapInfoCreate;
  rootStore.dispatch({ type: "GET_CREATE_ASSETMAPINFO", payload: rtn });
  setLoader("REMOVE", "GetAssetMapInfoCreateResource");
}

export async function CreatAssetMapInfo(data: AssetMapInfoDto) {
  setLoader("ADD", "CreatAssetMapInfo");
  let api = new AssetMapInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.AssetMapInfoCreate(data)
  );
  let rtn = {
    ResultDtoCreate: result,
    LookUpDtoCreate: null,
  } as AssetMapInfoCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "CREATE_ASSETMAPINFO", payload: rtn });
  setLoader("REMOVE", "CreatAssetMapInfo");
  return rtn;
}
