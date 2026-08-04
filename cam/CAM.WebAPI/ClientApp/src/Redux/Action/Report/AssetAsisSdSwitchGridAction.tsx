import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { AssetAsisSdSwitchApi } from "../../../Business/Report/AssetAsisSdSwitchBusiness";
import {
  AssetAsisSdSwitchGrid,
  AssetAsisSdSwitchQueryObjectGrid,
  QueryResultDtoOfAssetAsisSdSwitchDtoGrid,
  GET_GRID_ASSETASISSDSWITCH,
  GET_FILTER_ASSETASISSDSWITCH,
} from "../../../Model/Report/AssetAsisSdSwitchExport";
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

export async function GetAssetAsisSdSwitchGrid(
  queryFilter?: AssetAsisSdSwitchQueryObjectGrid
) {
  setLoader("ADD", "GetAssetAsisSdSwitchGrid");

  let result: QueryResultDtoOfAssetAsisSdSwitchDtoGrid | null | undefined;
  let api = new AssetAsisSdSwitchApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfAssetAsisSdSwitchDtoGrid>
    >(() => api.AssetAsisSdSwitchGetReport(queryFilter ?? {}));

    let rtn = {
      AssetAsisSdSwitchGridResult: result,
      filter: null,
    } as AssetAsisSdSwitchGrid;
    rootStore.dispatch({ type: GET_GRID_ASSETASISSDSWITCH, payload: rtn });
    // return rtn;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    let rtn = {
      AssetAsisSdSwitchGridResult: null,
      filter: null,
    } as AssetAsisSdSwitchGrid;
    rootStore.dispatch({ type: GET_GRID_ASSETASISSDSWITCH, payload: rtn });
  }
  setLoader("REMOVE", "GetAssetAsisSdSwitchGrid");
}

export async function GetFilterColumAssetAsisSdSwitch(
  columName: string,
  columValue: string,
  queryFilter?: AssetAsisSdSwitchQueryObjectGrid
) {
  let api = new AssetAsisSdSwitchApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.AssetAsisSdSwitchGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    AssetAsisSdSwitchGridResult: null,
  } as AssetAsisSdSwitchGrid;
  rootStore.dispatch({ type: GET_FILTER_ASSETASISSDSWITCH, payload: rtn });
}
