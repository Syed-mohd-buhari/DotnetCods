import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { AssetMapInfoApi } from "../../../../Business/LookUp/AssetMapInfoBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  AssetMapInfoGrid,
  AssetMapInfoQueryObjectGrid,
  QueryResultDtoOfAssetMapInfoDtoGrid,
} from "../../../../Model/LookUp/AssetMapInfo";

export async function GetAssetMapInfoGrid(
  queryFilter?: AssetMapInfoQueryObjectGrid
) {
  setLoader("ADD", "GetAssetMapInfoGrid");

  let result: QueryResultDtoOfAssetMapInfoDtoGrid | null | undefined;
  let api = new AssetMapInfoApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfAssetMapInfoDtoGrid>
      >(() => api.AssetMapInfoGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfAssetMapInfoDtoGrid>
      >(() => api.AssetMapInfoGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as AssetMapInfoGrid;
    rootStore.dispatch({
      type: "GET_GRID_ASSETMAPINFO",
      payload: rtn as AssetMapInfoGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_ASSETMAPINFO",
      payload: { LookUpGridResult: result, filter: null } as AssetMapInfoGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetAssetMapInfoGrid");
}

export async function GetAssetMapInfoGridALL(queryFilter) {
  setLoader("ADD", "GetAssetMapInfoGridALL");

  let result: QueryResultDtoOfAssetMapInfoDtoGrid | null | undefined;
  let api = new AssetMapInfoApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfAssetMapInfoDtoGrid>
    >(() => api.AssetMapInfoGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as AssetMapInfoGrid;
    rootStore.dispatch({
      type: "GET_GRID_ASSETMAPINFO_ALL",
      payload: rtn as AssetMapInfoGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_ASSETMAPINFO_ALL",
      payload: { LookUpGridResult: result, filter: null } as AssetMapInfoGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetAssetMapInfoGridALL");
}

export async function GetFilterColumAssetMapInfo(
  columName: string,
  columValue: string,
  queryFilter?: AssetMapInfoQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new AssetMapInfoApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.AssetMapInfoGetFilterResult(
        columName,
        columValue,
        queryFilter?.omcAssetName,
        queryFilter?.temsAssetName,
        queryFilter?.enmAssetName,
        queryFilter?.site,
        queryFilter?.dataSourceName,
        queryFilter?.assetMapInfoId,

        queryFilter?.sortBy,
        queryFilter?.isSortAscending,
        queryFilter?.page,
        queryFilter?.pageSize,
        queryFilter?.lastModified?.startDate,
        queryFilter?.lastModified?.endDate,
        queryFilter?.principalId,
        queryFilter?.deleted,
        queryFilter?.orphan,
        queryFilter?.lastModifiedBy
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.AssetMapInfoGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as AssetMapInfoGrid;
  rootStore.dispatch({ type: "GET_FILTER_ASSETMAPINFO", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
