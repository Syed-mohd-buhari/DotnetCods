import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { AssetAsisHwAncillaryApi } from "../../../Business/Report/AssetAsisHwAncillaryBusiness";

import {
  AssetAsisHwAncillaryGrid,
  AssetAsisHwAncillaryQueryObjectGrid,
  QueryResultDtoOfAssetAsisHwAncillaryDtoGrid,
  GET_GRID_ASSETASISHWANCILLARY,
  GET_FILTER_ASSETASISHWANCILLARY,
} from "../../../Model/Report/AssetAsisHwAncillaryExport";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import {
  LookUpGrid,
  QueryResultDtoOfTipologicaGridDto,
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../../../Model/LookUp/LookUpGenericModel";
import { SharedLookUpApi } from "../../../Business/LookUp/SharedLookUpBusiness";
import { TSRReportApi } from "../../../Business/TSRReportBusiness";

export async function GetAssetAsisHwAncillaryGrid(
  queryFilter?: AssetAsisHwAncillaryQueryObjectGrid
) {
  setLoader("ADD", "GetAssetAsisHwAncillaryGrid");

  let result: QueryResultDtoOfAssetAsisHwAncillaryDtoGrid | null | undefined;
  let api = new AssetAsisHwAncillaryApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfAssetAsisHwAncillaryDtoGrid>
    >(() => api.AssetAsisHwAncillaryGetReport(queryFilter ?? {}));

    let rtn = {
      AssetAsisHwAncillaryGridResult: result,
      filter: null,
    } as AssetAsisHwAncillaryGrid;
    rootStore.dispatch({
      type: GET_GRID_ASSETASISHWANCILLARY,
      payload: rtn,
    });
    // return rtn;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    let rtn = {
      AssetAsisHwAncillaryGridResult: null,
      filter: null,
    } as AssetAsisHwAncillaryGrid;
    rootStore.dispatch({
      type: GET_GRID_ASSETASISHWANCILLARY,
      payload: rtn,
    });
  }
  setLoader("REMOVE", "GetAssetAsisHwAncillaryGrid");
}

export async function GetFilterColumAssetAsisHwAncillary(
  columName: string,
  columValue: string,
  queryFilter?: AssetAsisHwAncillaryQueryObjectGrid
) {
  let api = new AssetAsisHwAncillaryApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.AssetAsisHwAncillaryGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    AssetAsisHwAncillaryGridResult: null,
  } as AssetAsisHwAncillaryGrid;
  rootStore.dispatch({
    type: GET_FILTER_ASSETASISHWANCILLARY,
    payload: rtn,
  });
}
