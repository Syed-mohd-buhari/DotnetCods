import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { CriticalAssetTypeApi } from "../../../../Business/LookUp/CriticalAssetType";
import {
  CriticalAssetTypeQueryObjectGrid,
  QueryResultDtoOfCriticalAssetTypeDtoGrid,
} from "../../../../Model/LookUp/CriticalAssetType";
import {
  TipologicaGridDto,
  QueryResultDtoOfTipologicaGridDto,
  LookUpGrid,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetCriticalAssetTypeGrid(
  queryFilter?: CriticalAssetTypeQueryObjectGrid
) {
  setLoader("ADD", "GetCriticalAssetTypeGrid");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new CriticalAssetTypeApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfCriticalAssetTypeDtoGrid>
      >(() =>
        api.criticalAssetTypeGetCriticalAssetType(
          queryFilter?.id,
          queryFilter?.description,
          queryFilter?.assetCategoryId,
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
        Promise<QueryResultDtoOfCriticalAssetTypeDtoGrid>
      >(() => api.criticalAssetTypeGetCriticalAssetType());
    }

    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_CRITICAL_ASSET_TYPE",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_CRITICAL_ASSET_TYPE",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetCriticalAssetTypeGrid");
}

export async function GetCriticalAssetTypeGridALL() {
  setLoader("ADD", "GetCriticalAssetTypeGridALL");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new CriticalAssetTypeApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfCriticalAssetTypeDtoGrid>
    >(() => api.criticalAssetTypeGetCriticalAssetType());
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_CRITICAL_ASSET_TYPE_ALL",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_CRITICAL_ASSET_TYPE_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetCriticalAssetTypeGridALL");
}

export async function GetFilterColumCriticalAssetType(
  columName: string,
  columValue: string,
  queryFilter?: CriticalAssetTypeQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumCriticalAssetType");

  let result: FilterValueDto[] | undefined;
  let api = new CriticalAssetTypeApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.criticalAssetTypeGetFilterResult(
        columName,
        columValue,
        queryFilter?.id,
        queryFilter?.description,
        queryFilter?.assetCategoryId,
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
      api.criticalAssetTypeGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({ type: "GET_FILTER_CRITICAL_ASSET_TYPE", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumCriticalAssetType");
}
