import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { PodTypeInfoApi } from "../../../../Business/LookUp/PodTypeInfoBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  PodTypeInfoGrid,
  PodTypeInfoQueryObjectGrid,
  QueryResultDtoOfPodTypeInfoDtoGrid,
} from "../../../../Model/LookUp/PodTypeInfo";

export async function GetPodTypeInfoGrid(
  queryFilter?: PodTypeInfoQueryObjectGrid
) {
  setLoader("ADD", "GetPodTypeInfoGrid");

  let result: QueryResultDtoOfPodTypeInfoDtoGrid | null | undefined;
  let api = new PodTypeInfoApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfPodTypeInfoDtoGrid>
      >(() => api.PodTypeInfoGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfPodTypeInfoDtoGrid>
      >(() => api.PodTypeInfoGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as PodTypeInfoGrid;
    rootStore.dispatch({
      type: "GET_GRID_PODTYPEINFO",
      payload: rtn as PodTypeInfoGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_PODTYPEINFO",
      payload: { LookUpGridResult: result, filter: null } as PodTypeInfoGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetPodTypeInfoGrid");
}

export async function GetPodTypeInfoGridALL(queryFilter) {
  setLoader("ADD", "GetPodTypeInfoGridALL");

  let result: QueryResultDtoOfPodTypeInfoDtoGrid | null | undefined;
  let api = new PodTypeInfoApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfPodTypeInfoDtoGrid>
    >(() => api.PodTypeInfoGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as PodTypeInfoGrid;
    rootStore.dispatch({
      type: "GET_GRID_PODTYPEINFO_ALL",
      payload: rtn as PodTypeInfoGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_PODTYPEINFO_ALL",
      payload: { LookUpGridResult: result, filter: null } as PodTypeInfoGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetPodTypeInfoGridALL");
}

export async function GetFilterColumPodTypeInfo(
  columName: string,
  columValue: string,
  queryFilter?: PodTypeInfoQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new PodTypeInfoApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.PodTypeInfoGetFilterResult(
        columName,
        columValue,
        queryFilter?.podRoleDescription,
        queryFilter?.podTypeInfoName,
        queryFilter?.podTypeInfoId,
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
      api.PodTypeInfoGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as PodTypeInfoGrid;
  rootStore.dispatch({ type: "GET_FILTER_PODTYPEINFO", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
