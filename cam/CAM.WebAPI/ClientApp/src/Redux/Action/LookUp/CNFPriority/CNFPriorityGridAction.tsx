import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { CNFPriorityApi } from "../../../../Business/LookUp/CNFPriorityBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  CNFPriorityGrid,
  CNFPriorityQueryObjectGrid,
  QueryResultDtoOfCNFPriorityDtoGrid,
} from "../../../../Model/LookUp/CNFPriority";

export async function GetCNFPriorityGrid(
  queryFilter?: CNFPriorityQueryObjectGrid
) {
  setLoader("ADD", "GetLocationGrid");

  let result: QueryResultDtoOfCNFPriorityDtoGrid | null | undefined;
  let api = new CNFPriorityApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfCNFPriorityDtoGrid>
      >(() => api.CNFPriorityGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfCNFPriorityDtoGrid>
      >(() => api.CNFPriorityGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as CNFPriorityGrid;
    rootStore.dispatch({
      type: "GET_GRID_CNFPRIORITY",
      payload: rtn as CNFPriorityGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_CNFPRIORITY",
      payload: { LookUpGridResult: result, filter: null } as CNFPriorityGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetCNFPriorityGrid");
}

export async function GetCNFPriorityGridALL(queryFilter) {
  setLoader("ADD", "GetCNFPriorityGridALL");

  let result: QueryResultDtoOfCNFPriorityDtoGrid | null | undefined;
  let api = new CNFPriorityApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfCNFPriorityDtoGrid>
    >(() => api.CNFPriorityGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as CNFPriorityGrid;
    rootStore.dispatch({
      type: "GET_GRID_CNFPRIORITY_ALL",
      payload: rtn as CNFPriorityGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_CNFPRIORITY_ALL",
      payload: { LookUpGridResult: result, filter: null } as CNFPriorityGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetCNFPriorityGridALL");
}

export async function GetFilterColumCNFPriority(
  columName: string,
  columValue: string,
  queryFilter?: CNFPriorityQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new CNFPriorityApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.CNFPriorityGetFilterResult(
        columName,
        columValue,
        queryFilter?.description,
        queryFilter?.cnfPriorityId,
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
      api.CNFPriorityGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as CNFPriorityGrid;
  rootStore.dispatch({ type: "GET_FILTER_CNFPRIORITY", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
