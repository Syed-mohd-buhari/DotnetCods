import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { AssetAsisSdApi } from "../../../Business/Report/AssetAsisSdBusiness";
import {
  AssetAsisSdGrid,
  AssetAsisSdQueryObjectGrid,
  QueryResultDtoOfAssetAsisSdDtoGrid,
  GET_GRID_ASSETASISSD,
  GET_FILTER_ASSETASISSD,
} from "../../../Model/Report/AssetAsisSdExport";
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
import { ReportSoftwareApi } from "../../../Business/Report/ReportSoftwareBusiness";
import { RiTentLine } from "react-icons/ri";

export async function GetAssetAsisSdGrid(
  queryFilter?: AssetAsisSdQueryObjectGrid
) {
  setLoader("ADD", "GetAssetAsisSdGrid");

  let result: QueryResultDtoOfAssetAsisSdDtoGrid | null | undefined;
  let api = new AssetAsisSdApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfAssetAsisSdDtoGrid>
    >(() => api.AssetAsisSdGetReport(queryFilter ?? {}));

    let rtn = {
      AssetAsisSdGridResult: result,
      filter: null,
    } as AssetAsisSdGrid;
    rootStore.dispatch({ type: GET_GRID_ASSETASISSD, payload: rtn });
    // return rtn;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    let rtn = {
      AssetAsisSdGridResult: null,
      filter: null,
    } as AssetAsisSdGrid;
    rootStore.dispatch({ type: GET_GRID_ASSETASISSD, payload: rtn });
  }
  setLoader("REMOVE", "GetAssetAsisSdGrid");
}

export async function GetFilterColumAssetAsisSd(
  columName: string,
  columValue: string,
  queryFilter?: AssetAsisSdQueryObjectGrid
) {
  let api = new AssetAsisSdApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.AssetAsisSdGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    AssetAsisSdGridResult: null,
  } as AssetAsisSdGrid;
  rootStore.dispatch({ type: GET_FILTER_ASSETASISSD, payload: rtn });
}
