import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { VNFClusterNameApi } from "../../../../Business/LookUp/VNFClusterNameBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  VNFClusterNameGrid,
  VNFClusterNameQueryObjectGrid,
  QueryResultDtoOfVNFClusterNameDtoGrid,
} from "../../../../Model/LookUp/VNFClusterName";

export async function GetVNFClusterNameGrid(
  queryFilter?: VNFClusterNameQueryObjectGrid
) {
  setLoader("ADD", "GetVNFClusterNameGrid");

  let result: QueryResultDtoOfVNFClusterNameDtoGrid | null | undefined;
  let api = new VNFClusterNameApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfVNFClusterNameDtoGrid>
      >(() => api.VNFClusterNameGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfVNFClusterNameDtoGrid>
      >(() => api.VNFClusterNameGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as VNFClusterNameGrid;
    rootStore.dispatch({
      type: "GET_GRID_VNFCLUSTERNAME",
      payload: rtn as VNFClusterNameGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_VNFCLUSTERNAME",
      payload: { LookUpGridResult: result, filter: null } as VNFClusterNameGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetVNFClusterNameGrid");
}

export async function GetVNFClusterNameGridALL(queryFilter) {
  setLoader("ADD", "GetVNFClusterNameGridALL");

  let result: QueryResultDtoOfVNFClusterNameDtoGrid | null | undefined;
  let api = new VNFClusterNameApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfVNFClusterNameDtoGrid>
    >(() => api.VNFClusterNameGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as VNFClusterNameGrid;
    rootStore.dispatch({
      type: "GET_GRID_VNFCLUSTERNAME_ALL",
      payload: rtn as VNFClusterNameGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_VNFCLUSTERNAME_ALL",
      payload: { LookUpGridResult: result, filter: null } as VNFClusterNameGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetVNFClusterNameGridALL");
}

export async function GetFilterColumVNFClusterName(
  columName: string,
  columValue: string,
  queryFilter?: VNFClusterNameQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new VNFClusterNameApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.VNFClusterNameGetFilterResult(
        columName,
        columValue,
        queryFilter?.clusterDescription,
        queryFilter?.clusterNameId,
        queryFilter?.clusterType,
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
      api.VNFClusterNameGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as VNFClusterNameGrid;
  rootStore.dispatch({ type: "GET_FILTER_VNFCLUSTERNAME", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
