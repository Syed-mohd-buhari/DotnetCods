import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import {
  TipologicaGridDto,
  QueryResultDtoOfTipologicaGridDto,
  TipologicheQueryObjectGrid,
  LookUpGrid,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import { BusinessContinuityMethodApi } from "../../../../Business/LookUp/BusinessContinuityMethodBusiness";
// import { useDispatch } from 'react-redux'

export async function GetBusinessContinuityMethodGrid(
  queryFilter?: TipologicheQueryObjectGrid
) {
  setLoader("ADD", "GetBusinessContinuityMethodGrid");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new BusinessContinuityMethodApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() =>
        api.businessContinuityMethodGetBusinessContinuityMethod(
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
          queryFilter?.id,
          queryFilter?.description
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfTipologicaGridDto>
      >(() => api.businessContinuityMethodGetBusinessContinuityMethod());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_GEO_RESILINCE",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_GEO_RESILINCE",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetBusinessContinuityMethodGrid");
}

export async function GetBusinessContinuityMethodGridALL() {
  setLoader("ADD", "GetBusinessContinuityMethodGridALL");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new BusinessContinuityMethodApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfTipologicaGridDto>
    >(() => api.businessContinuityMethodGetBusinessContinuityMethod());
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_GEO_RESILINCE_ALL",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_GEO_RESILINCE_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetBusinessContinuityMethodGridALL");
}

export async function GetFilterColumBusinessContinuityMethod(
  columName: string,
  columValue: string,
  queryFilter?: TipologicheQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumBusinessContinuityMethod");

  let result: FilterValueDto[] | undefined;
  let api = new BusinessContinuityMethodApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.businessContinuityMethodGetFilterResult(
        columName,
        columValue,
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
        queryFilter?.id,
        queryFilter?.description
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.businessContinuityMethodGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({
    type: "GET_FILTER_GEO_RESILINCE",
    payload: rtn,
  });
  // setLoader("REMOVE", "GetFilterColumBusinessContinuityMethod");
}
