import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { CNFClusterApi } from "../../../../Business/LookUp/CNFClusterBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  CNFClusterGrid,
  CNFClusterQueryObjectGrid,
  QueryResultDtoOfCNFClusterDtoGrid,
} from "../../../../Model/LookUp/CNFCluster";

export async function GetCNFClusterGrid(
  queryFilter?: CNFClusterQueryObjectGrid
) {
  setLoader("ADD", "GetCNFClusterGrid");

  let result: QueryResultDtoOfCNFClusterDtoGrid | null | undefined;
  let api = new CNFClusterApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfCNFClusterDtoGrid>
      >(() => api.CNFClusterGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfCNFClusterDtoGrid>
      >(() => api.CNFClusterGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as CNFClusterGrid;
    rootStore.dispatch({
      type: "GET_GRID_CNFCLUSTER",
      payload: rtn as CNFClusterGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_CNFCLUSTER",
      payload: { LookUpGridResult: result, filter: null } as CNFClusterGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetCNFClusterGrid");
}

export async function GetCNFClusterGridALL(queryFilter) {
  setLoader("ADD", "GetCNFClusterGridALL");

  let result: QueryResultDtoOfCNFClusterDtoGrid | null | undefined;
  let api = new CNFClusterApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfCNFClusterDtoGrid>
    >(() => api.CNFClusterGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as CNFClusterGrid;
    rootStore.dispatch({
      type: "GET_GRID_CNFCLUSTER_ALL",
      payload: rtn as CNFClusterGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_CNFCLUSTER_ALL",
      payload: { LookUpGridResult: result, filter: null } as CNFClusterGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetCNFClusterGridALL");
}

export async function GetFilterColumCNFCluster(
  columName: string,
  columValue: string,
  queryFilter?: CNFClusterQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new CNFClusterApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.CNFClusterGetFilterResult(
        columName,
        columValue,
        queryFilter?.cnfClusterName,
        queryFilter?.cnfClusterId,
        queryFilter?.alaisName,
        queryFilter?.cnfName,
        queryFilter?.nodePool,
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
      api.CNFClusterGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as CNFClusterGrid;
  rootStore.dispatch({ type: "GET_FILTER_CNFCLUSTER", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
