import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { SupportedResourceApi } from "../../../../Business/LookUp/SupportedResourceBusiness";
import {
  TipologicaGridDto,
  QueryResultDtoOfTipologicaGridDto,
  TipologicheQueryObjectGrid,
  LookUpGrid,
  TipologicheQueryObjectGridRule,
  TipologicaGridDtoRule,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSupportedResourceGrid(
  queryFilter?: TipologicheQueryObjectGridRule
) {
  setLoader("ADD", "GetSupportedResourceGrid");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new SupportedResourceApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() =>
        api.supportedResourceGetSupportedResource(
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
          queryFilter?.rule
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() => api.supportedResourceGetSupportedResource());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = {
      LookUpGridResult: result,
      filter: null,
    } as TipologicaGridDtoRule;
    rootStore.dispatch({
      type: "GET_GRID_SUPPORTED_RESOURCE",
      payload: rtn as TipologicaGridDtoRule,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SUPPORTED_RESOURCE",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetSupportedResourceGrid");
}

export async function GetSupportedResourceGridALL() {
  setLoader("ADD", "GetSupportedResourceGridALL");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new SupportedResourceApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfTipologicaGridDto>
    >(() => api.supportedResourceGetSupportedResource());
    let rtn = {
      LookUpGridResult: result,
      filter: null,
    } as TipologicaGridDtoRule;
    rootStore.dispatch({
      type: "GET_GRID_SUPPORTED_RESOURCE_ALL",
      payload: rtn as TipologicaGridDtoRule,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SUPPORTED_RESOURCE_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetSupportedResourceGridALL");
}

export async function GetFilterColumSupportedResource(
  columName: string,
  columValue: string,
  queryFilter?: TipologicheQueryObjectGridRule
) {
  // setLoader("ADD", "GetFilterColumSupportedResource");

  let result: FilterValueDto[] | undefined;
  let api = new SupportedResourceApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.supportedResourceGetFilterResult(
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
        queryFilter?.rule
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.supportedResourceGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({ type: "GET_FILTER_SUPPORTED_RESOURCE", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumSupportedResource");
}
