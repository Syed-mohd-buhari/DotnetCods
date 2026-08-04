import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { CNFNameApi } from "../../../../Business/LookUp/CNFNameBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  CNFNameGrid,
  CNFNameQueryObjectGrid,
  QueryResultDtoOfCNFNameDtoGrid,
} from "../../../../Model/LookUp/CNFName";

export async function GetCNFNameGrid(queryFilter?: CNFNameQueryObjectGrid) {
  setLoader("ADD", "GetLocationGrid");

  let result: QueryResultDtoOfCNFNameDtoGrid | null | undefined;
  let api = new CNFNameApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfCNFNameDtoGrid>
      >(() => api.CNFNameGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfCNFNameDtoGrid>
      >(() => api.CNFNameGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as CNFNameGrid;
    rootStore.dispatch({
      type: "GET_GRID_CNFNAME",
      payload: rtn as CNFNameGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_CNFNAME",
      payload: { LookUpGridResult: result, filter: null } as CNFNameGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetCNFNameGrid");
}

export async function GetCNFNameGridALL(queryFilter) {
  setLoader("ADD", "GetCNFNameGridALL");

  let result: QueryResultDtoOfCNFNameDtoGrid | null | undefined;
  let api = new CNFNameApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfCNFNameDtoGrid>
    >(() => api.CNFNameGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as CNFNameGrid;
    rootStore.dispatch({
      type: "GET_GRID_CNFNAME_ALL",
      payload: rtn as CNFNameGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_CNFNAME_ALL",
      payload: { LookUpGridResult: result, filter: null } as CNFNameGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetCNFNameGridALL");
}

export async function GetFilterColumCNFName(
  columName: string,
  columValue: string,
  queryFilter?: CNFNameQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new CNFNameApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.CNFNameGetFilterResult(
        columName,
        columValue,
        queryFilter?.cnfDescription,
        queryFilter?.cnfNameId,
        queryFilter?.productId,
        queryFilter?.product,
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
      api.CNFNameGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as CNFNameGrid;
  rootStore.dispatch({ type: "GET_FILTER_CNFNAME", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
