import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { AssetClassApi } from "../../../../Business/LookUp/AssetClassBusiness";
import {
  AssetClassQueryObjectGrid,
  QueryResultDtoOfAssetClassDtoGrid,
} from "../../../../Model/LookUp/AssetClass";
import {
  TipologicaGridDto,
  QueryResultDtoOfTipologicaGridDto,
  LookUpGrid,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetAssetClassGrid(
  queryFilter?: AssetClassQueryObjectGrid
) {
  setLoader("ADD", "GetAssetClassGrid");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new AssetClassApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfAssetClassDtoGrid>
      >(() =>
        api.assetClassGetAssetClass(
          queryFilter?.id,
          queryFilter?.description,
          queryFilter?.sortBy,
          queryFilter?.isSortAscending,
          queryFilter?.page,
          queryFilter?.pageSize,
          queryFilter?.lastModifiedStartDate,
          queryFilter?.lastModifiedEndDate,
          queryFilter?.principalId,
          queryFilter?.deleted,
          queryFilter?.orphan,
          queryFilter?.lastModifiedBy
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfAssetClassDtoGrid>
      >(() => api.assetClassGetAssetClass());
    }
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_ASSET_CLASS",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_ASSET_CLASS",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetAssetClassGrid");
}

export async function GetAssetClassGridALL() {
  setLoader("ADD", "GetAssetClassGridALL");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new AssetClassApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfAssetClassDtoGrid>
    >(() => api.assetClassGetAssetClass());
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_ASSET_CLASS_ALL",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_ASSET_CLASS_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetAssetClassGridALL");
}

export async function GetFilterColumAssetClass(
  columName: string,
  columValue: string,
  queryFilter?: AssetClassQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumAssetClass");

  let result: FilterValueDto[] | undefined;
  let api = new AssetClassApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.assetClassGetFilterResult(
        columName,
        columValue,
        queryFilter?.id,
        queryFilter?.description,
        queryFilter?.sortBy,
        queryFilter?.isSortAscending,
        queryFilter?.page,
        queryFilter?.pageSize,
        queryFilter?.lastModifiedStartDate,
        queryFilter?.lastModifiedEndDate,
        queryFilter?.principalId,
        queryFilter?.deleted,
        queryFilter?.orphan,
        queryFilter?.lastModifiedBy
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.assetClassGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({ type: "GET_FILTER_ASSET_CLASS", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumAssetClass");
}
