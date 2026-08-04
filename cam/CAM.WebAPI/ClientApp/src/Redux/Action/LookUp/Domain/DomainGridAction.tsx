import React from "react";
import { ApiCallWithErrorHandling, FilterValueDto } from "../../../../Business/Common/CommonBusiness";
import { SystemNamesApi } from "../../../../Business/LookUp/DomainBuisness";

import {
  LookUpGridForSystemNames,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import { QueryResultDtoOfSystemNamesDtoGrid, SystemNameQuery, SystemNamesDtoGrid } from "../../../../Model/LookUp/Domain";

export async function GetSystemNameGrid(queryFilter?: SystemNameQuery) {
  setLoader("ADD", "GetSystemNameGrid");

  let result: QueryResultDtoOfSystemNamesDtoGrid | null | undefined;
  let api = new SystemNamesApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfSystemNamesDtoGrid>>(() =>
        api.systemNamesGetSystemNames(
          queryFilter?.systemNameId,
          queryFilter?.systemNameDescription,
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
          queryFilter?.options
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfSystemNamesDtoGrid>>(() => api.systemNamesGetSystemNames());
    }

    if (result?.items?.length === 0 || result?.totalItems === undefined) {
      rootStore.dispatch(setNotification({ message: "No results found", notifyType: NotifyType.error }));
    }

    let rtn = { LookUpGridResult: result, filter: null } as SystemNamesDtoGrid;
    rootStore.dispatch({ type: "GET_GRID_SYSTEM_NAME", payload: rtn as SystemNamesDtoGrid });
  } catch (error) {
    rootStore.dispatch({ type: "GET_GRID_SYSTEM_NAME", payload: { LookUpGridResult: result, filter: null } as LookUpGridForSystemNames });
    rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
  }
  setLoader("REMOVE", "GetSystemNameGrid");
}

export async function GetSystemNameGridALL() {
  setLoader("ADD", "GetSystemNameGridALL");

  let result: QueryResultDtoOfSystemNamesDtoGrid | null | undefined;
  let api = new SystemNamesApi();
  try {
    result = await ApiCallWithErrorHandling<Promise<QueryResultDtoOfSystemNamesDtoGrid>>(() => api.systemNamesGetSystemNames());
    let rtn = { LookUpGridResult: result, filter: null } as SystemNamesDtoGrid;
    rootStore.dispatch({ type: "GET_GRID_SYSTEM_NAME_ALL", payload: rtn as SystemNamesDtoGrid });
  } catch (error) {
    rootStore.dispatch({ type: "GET_GRID_SYSTEM_NAME_ALL", payload: { LookUpGridResult: result, filter: null } as LookUpGridForSystemNames });
    rootStore.dispatch(setNotification({ message: "Fail to fetch", notifyType: NotifyType.error }));
  }
  setLoader("REMOVE", "GetSystemNameGridALL");
}

export async function GetFilterColumSystemName(columName: string, columValue: string, queryFilter?: SystemNameQuery) {
  // setLoader("ADD", "GetFilterColumSystemName");

  let result: FilterValueDto[] | undefined;
  let api = new SystemNamesApi();

  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.systemNamesGetFilterResult(
        columName,
        columValue,
        queryFilter?.systemNameId,
        queryFilter?.systemNameDescription,
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
        queryFilter?.options
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() => api.systemNamesGetFilterResult(columName, columValue));
  }

  let rtn = { filter: result, LookUpGridResult: null } as LookUpGridForSystemNames;
  rootStore.dispatch({ type: "GET_FILTER_SYSTEM_NAME", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumSystemName");
}
