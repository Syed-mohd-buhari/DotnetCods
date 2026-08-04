import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { BuildConstructionApi } from "../../../../Business/LookUp/BuildConstructionBusiness";
import {
  TipologicaGridDto,
  QueryResultDtoOfTipologicaGridDto,
  TipologicheQueryObjectGrid,
  LookUpGrid,
  TipologicheQueryObjectGridRule,
  QueryResultDtoOfTipologicaGridDtoRule,
  TipologicaGridDtoRule,
  LookUpGridRule,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetBuildConstructionGrid(
  queryFilter?: TipologicheQueryObjectGridRule
) {
  setLoader("ADD", "GetBuildConstructionGrid");

  let result: QueryResultDtoOfTipologicaGridDtoRule | null | undefined;
  let api = new BuildConstructionApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDtoRule>
      >(() =>
        api.buildConstructionGetBuildConstruction(
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
          queryFilter?.lastModifiedBy,
          queryFilter?.rule,
          queryFilter?.cloudTypeBuild,
          queryFilter?.isCloudHostedAsset
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDtoRule>
      >(() => api.buildConstructionGetBuildConstruction());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = {
      LookUpGridResult: result,
      filter: null,
    } as TipologicaGridDtoRule;
    rootStore.dispatch({
      type: "GET_GRID_BUILD_CONSTRUCTION",
      payload: rtn as TipologicaGridDtoRule,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_BUILD_CONSTRUCTION",
      payload: { LookUpGridResult: result, filter: null } as LookUpGridRule,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetBuildConstructionGrid");
}

export async function GetBuildConstructionGridALL() {
  setLoader("ADD", "GetBuildConstructionGridALL");

  let result: QueryResultDtoOfTipologicaGridDtoRule | null | undefined;
  let api = new BuildConstructionApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfTipologicaGridDtoRule>
    >(() => api.buildConstructionGetBuildConstruction());
    let rtn = {
      LookUpGridResult: result,
      filter: null,
    } as TipologicaGridDtoRule;
    rootStore.dispatch({
      type: "GET_GRID_BUILD_CONSTRUCTION_ALL",
      payload: rtn as TipologicaGridDtoRule,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_BUILD_CONSTRUCTION_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGridRule,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetBuildConstructionGridALL");
}

export async function GetFilterColumBuildConstruction(
  columName: string,
  columValue: string,
  queryFilter?: TipologicheQueryObjectGridRule
) {
  // setLoader("ADD", "GetFilterColumBuildConstruction");

  let result: FilterValueDto[] | undefined;
  let api = new BuildConstructionApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.buildConstructionGetFilterResult(
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
        queryFilter?.lastModifiedBy,
        queryFilter?.rule,
        queryFilter?.cloudTypeBuild,
        queryFilter?.isCloudHostedAsset
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.buildConstructionGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({ type: "GET_FILTER_BUILD_CONSTRUCTION", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumBuildConstruction");
}
