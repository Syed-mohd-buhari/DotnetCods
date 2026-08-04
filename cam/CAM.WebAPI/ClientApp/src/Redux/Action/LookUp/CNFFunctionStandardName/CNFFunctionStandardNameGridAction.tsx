import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { CNFFunctionStandardNameApi } from "../../../../Business/LookUp/CNFFunctionStandardNameBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  CNFFunctionStandardNameGrid,
  CNFFunctionStandardNameQueryObjectGrid,
  QueryResultDtoOfCNFFunctionStandardNameDtoGrid,
} from "../../../../Model/LookUp/CNFFunctionStandardName";

export async function GetCNFFunctionStandardNameGrid(
  queryFilter?: CNFFunctionStandardNameQueryObjectGrid
) {
  setLoader("ADD", "GetCNFFunctionStandardNameGrid");

  let result: QueryResultDtoOfCNFFunctionStandardNameDtoGrid | null | undefined;
  let api = new CNFFunctionStandardNameApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfCNFFunctionStandardNameDtoGrid>
      >(() => api.CNFFunctionStandardNameGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfCNFFunctionStandardNameDtoGrid>
      >(() => api.CNFFunctionStandardNameGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = {
      LookUpGridResult: result,
      filter: null,
    } as CNFFunctionStandardNameGrid;
    rootStore.dispatch({
      type: "GET_GRID_CNFFUNCTIONSTANDARDNAME",
      payload: rtn as CNFFunctionStandardNameGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_CNFFUNCTIONSTANDARDNAME",
      payload: {
        LookUpGridResult: result,
        filter: null,
      } as CNFFunctionStandardNameGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetCNFFunctionStandardNameGrid");
}

export async function GetCNFFunctionStandardNameGridALL(queryFilter) {
  setLoader("ADD", "GetCNFFunctionStandardNameGridALL");

  let result: QueryResultDtoOfCNFFunctionStandardNameDtoGrid | null | undefined;
  let api = new CNFFunctionStandardNameApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfCNFFunctionStandardNameDtoGrid>
    >(() => api.CNFFunctionStandardNameGet(queryFilter));
    let rtn = {
      LookUpGridResult: result,
      filter: null,
    } as CNFFunctionStandardNameGrid;
    rootStore.dispatch({
      type: "GET_GRID_CNFFUNCTIONSTANDARDNAME_ALL",
      payload: rtn as CNFFunctionStandardNameGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_CNFFUNCTIONSTANDARDNAME_ALL",
      payload: {
        LookUpGridResult: result,
        filter: null,
      } as CNFFunctionStandardNameGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetCNFFunctionStandardNameGridALL");
}

export async function GetFilterColumCNFFunctionStandardName(
  columName: string,
  columValue: string,
  queryFilter?: CNFFunctionStandardNameQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new CNFFunctionStandardNameApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.CNFFunctionStandardNameGetFilterResult(
        columName,
        columValue,
        queryFilter?.functionName,
        queryFilter?.functionStandardNameId,
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
      api.CNFFunctionStandardNameGetFilterResult(columName, columValue)
    );
  }
  let rtn = {
    filter: result,
    LookUpGridResult: null,
  } as CNFFunctionStandardNameGrid;
  rootStore.dispatch({
    type: "GET_FILTER_CNFFUNCTIONSTANDARDNAME",
    payload: rtn,
  });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
