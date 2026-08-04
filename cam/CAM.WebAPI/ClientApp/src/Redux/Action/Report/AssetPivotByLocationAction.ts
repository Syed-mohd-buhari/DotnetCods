import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import {
  ReportPATGrid,
  ReportPATQueryObjectGrid,
  GET_FILTER_REPORT_PAT,
  SWOEM_MODEL,
} from "../../../Model/Report/PlannedActivityTrackerModel";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { AssetPivotByLocation } from "../../../Business/AssetPivotByLocationBusiness";
import { rootStore } from "../../Store/rootStore";
import { setNotification } from "../NotificationAction";
import setLoader from "../LoaderAction";
import { FileResult, ReturnFile } from "../../../Model/Common";
import {
  AssetPivotByLocationDto,
  AssetPivotByLocationGrid,
  AssetPivotByLocationQueryObjectGrid,
  GET_ASSET_BY_LOCATION,
  GET_FILTER_ASSET_BY_LOCATION,
} from "../../../Model/Report/AssetPivotByLocationModel";
// import { useDispatch } from 'react-redux'

export async function GetAssetPivotByLocationGrid(
  queryFilter?: AssetPivotByLocationQueryObjectGrid
) {
  setLoader("ADD", "GetAssetPivotByLocationGrid");
  let result: AssetPivotByLocationDto | null | undefined;
  let api = new AssetPivotByLocation();

  try {
    result = await ApiCallWithErrorHandling<Promise<AssetPivotByLocationDto>>(
      () => api.AssetPivotByLocationGetAll(queryFilter)
    );

    let rtn = {
      AssetPivotByLocationGridResult: result,
      filter: null,
    } as AssetPivotByLocationGrid;
    rootStore.dispatch({ type: GET_ASSET_BY_LOCATION, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    var rtn = {
      AssetPivotByLocationGridResult: null,
      filter: null,
    } as AssetPivotByLocationGrid;
    rootStore.dispatch({ type: GET_ASSET_BY_LOCATION, payload: rtn });
  }

  setLoader("REMOVE", "GetAssetPivotByLocationGrid");
}

export async function GetAssetPivotByLocationExport(
  queryFilter?: AssetPivotByLocationQueryObjectGrid
) {
  setLoader("ADD", "GetAssetPivotByLocationExport");
  let api = new AssetPivotByLocation();

  let res = await ApiCallWithErrorHandling<Promise<ReturnFile>>(() =>
    api.AssetPivotByLocationExport(
      queryFilter?.opCo,
      queryFilter?.dcId,
      queryFilter?.hardwareType,
      queryFilter?.deploymentStatus,
      queryFilter?.paImplementaionYear,
      queryFilter?.sortBy,
      queryFilter?.isSortAscending,
      queryFilter?.page,
      queryFilter?.pageSize,
      queryFilter?.principalId,
      queryFilter?.deleted,
      queryFilter?.orphan,
      queryFilter?.lastModified
    )
  );
  let fileNameBase = res?.FileName.split(";")[1] ?? "";
  var index = fileNameBase?.indexOf('"') + 1;
  var lastIndex = fileNameBase?.indexOf('"', index);
  let fileName = fileNameBase?.substring(index, lastIndex);
  let result = res?.File.then((x) => {
    return { file: x, fileName: fileName } as FileResult;
  });
  setLoader("REMOVE", "GetAssetPivotByLocationExport");
  return result;
}

export async function GetFilterColumnAssetPivotByLocation(
  columName: string,
  columValue: string,
  queryFilter?: AssetPivotByLocationQueryObjectGrid
) {
  let api = new AssetPivotByLocation();

  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.AssetPivotByLocationFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  var rtn = {
    filter: result,
    AssetPivotByLocationGridResult: null,
  } as AssetPivotByLocationGrid;
  rootStore.dispatch({ type: GET_FILTER_ASSET_BY_LOCATION, payload: rtn });
}

export async function GetDropdownData() {
  setLoader("ADD", "getDropdownData");
  let api = new AssetPivotByLocation();

  let rtn = await ApiCallWithErrorHandling<Promise<any>>(() =>
    api.getDropdownData()
  );

  setLoader("REMOVE", "getDropdownData");
  return rtn;
}
