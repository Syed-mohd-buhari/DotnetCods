import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { VMTypeNameApi } from "../../../../Business/LookUp/VMTypeNameBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  VMTypeNameGrid,
  VMTypeNameQueryObjectGrid,
  QueryResultDtoOfVMTypeNameDtoGrid,
} from "../../../../Model/LookUp/VMTypeName";

export async function GetVMTypeNameGrid(
  queryFilter?: VMTypeNameQueryObjectGrid
) {
  setLoader("ADD", "GetLocationGrid");

  let result: QueryResultDtoOfVMTypeNameDtoGrid | null | undefined;
  let api = new VMTypeNameApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfVMTypeNameDtoGrid>
      >(() => api.VMTypeNameGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfVMTypeNameDtoGrid>
      >(() => api.VMTypeNameGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as VMTypeNameGrid;
    rootStore.dispatch({
      type: "GET_GRID_VMTYPENAME",
      payload: rtn as VMTypeNameGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_VMTYPENAME",
      payload: { LookUpGridResult: result, filter: null } as VMTypeNameGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetVMTypeNameGrid");
}

export async function GetVMTypeNameGridALL(queryFilter) {
  setLoader("ADD", "GetVMTypeNameGridALL");

  let result: QueryResultDtoOfVMTypeNameDtoGrid | null | undefined;
  let api = new VMTypeNameApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfVMTypeNameDtoGrid>
    >(() => api.VMTypeNameGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as VMTypeNameGrid;
    rootStore.dispatch({
      type: "GET_GRID_VMTYPENAME_ALL",
      payload: rtn as VMTypeNameGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_VMTYPENAME_ALL",
      payload: { LookUpGridResult: result, filter: null } as VMTypeNameGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetVMTypeNameGridALL");
}

export async function GetFilterColumVMTypeName(
  columName: string,
  columValue: string,
  queryFilter?: VMTypeNameQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new VMTypeNameApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.VMTypeNameGetFilterResult(
        columName,
        columValue,
        queryFilter?.vmTypeDescription,
        queryFilter?.vmTypeNameId,
        queryFilter?.vnfName,
        queryFilter?.vnfNameId,
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
      api.VMTypeNameGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as VMTypeNameGrid;
  rootStore.dispatch({ type: "GET_FILTER_VMTYPENAME", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
