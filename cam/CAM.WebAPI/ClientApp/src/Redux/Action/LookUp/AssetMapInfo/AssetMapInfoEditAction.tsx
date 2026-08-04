import React from "react";
import { ApiCallWithErrorHandling } from "../../../../Business/Common/CommonBusiness";
import { AssetMapInfoApi } from "../../../../Business/LookUp/AssetMapInfoBusiness";
import { ResultDto } from "../../../../Model/CommonModels";
import {
  AssetMapInfoDto,
  AssetMapInfoDtoGrid,
  AssetMapInfoEdit,
} from "../../../../Model/LookUp/AssetMapInfo";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetAssetMapInfoEditResource(id: number) {
  setLoader("ADD", "GetAssetMapInfoEditResource");

  let api = new AssetMapInfoApi();
  let createResource = await ApiCallWithErrorHandling<Promise<AssetMapInfoDto>>(
    () => api.AssetMapInfoGetUpdatedPage(id)
  );
  let rtn = { LookUpDtoEdit: createResource } as AssetMapInfoEdit;
  rootStore.dispatch({ type: "GET_EDIT_ASSETMAPINFO", payload: rtn });
  setLoader("REMOVE", "GetAssetMapInfoEditResource");

  return rtn;
}

export async function EditAssetMapInfo(data: AssetMapInfoDto) {
  setLoader("ADD", "EditAssetMapInfo");
  let api = new AssetMapInfoApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.AssetMapInfoUpdate(data)
  );
  let rtn = { ResultDtoEdit: result } as AssetMapInfoEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "EDIT_ASSETMAPINFO", payload: rtn });
  setLoader("REMOVE", "EditAssetMapInfo");
  return rtn;
}
