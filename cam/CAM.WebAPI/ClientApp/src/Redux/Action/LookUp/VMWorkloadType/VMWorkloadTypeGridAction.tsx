import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { VMWorkloadTypeApi } from "../../../../Business/LookUp/VMWorkloadTypeBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  VMWorkloadTypeGrid,
  VMWorkloadTypeQueryObjectGrid,
  QueryResultDtoOfVMWorkloadTypeDtoGrid,
} from "../../../../Model/LookUp/VMWorkloadType";

export async function GetVMWorkloadTypeGrid(
  queryFilter?: VMWorkloadTypeQueryObjectGrid
) {
  setLoader("ADD", "GetVMWorkloadGrid");

  let result: QueryResultDtoOfVMWorkloadTypeDtoGrid | null | undefined;
  let api = new VMWorkloadTypeApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfVMWorkloadTypeDtoGrid>
      >(() => api.VMWorkloadTypeGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfVMWorkloadTypeDtoGrid>
      >(() => api.VMWorkloadTypeGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as VMWorkloadTypeGrid;
    rootStore.dispatch({
      type: "GET_GRID_VMWORKLOADTYPE",
      payload: rtn as VMWorkloadTypeGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_VMWORKLOADTYPE",
      payload: { LookUpGridResult: result, filter: null } as VMWorkloadTypeGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetVMWorkloadTypeGrid");
}

export async function GetVMWorkloadTypeGridALL(queryFilter) {
  setLoader("ADD", "GetVMWorkloadTypeGridALL");

  let result: QueryResultDtoOfVMWorkloadTypeDtoGrid | null | undefined;
  let api = new VMWorkloadTypeApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfVMWorkloadTypeDtoGrid>
    >(() => api.VMWorkloadTypeGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as VMWorkloadTypeGrid;
    rootStore.dispatch({
      type: "GET_GRID_VMWORKLOADTYPE_ALL",
      payload: rtn as VMWorkloadTypeGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_VMWORKLOADTYPE_ALL",
      payload: { LookUpGridResult: result, filter: null } as VMWorkloadTypeGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetVMWorkloadTypeGridALL");
}

export async function GetFilterColumVMWorkloadType(
  columName: string,
  columValue: string,
  queryFilter?: VMWorkloadTypeQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new VMWorkloadTypeApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.VMWorkloadTypeGetFilterResult(
        columName,
        columValue,
        queryFilter?.description,
        queryFilter?.vmWorkLoadTypeId,
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
      api.VMWorkloadTypeGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as VMWorkloadTypeGrid;
  rootStore.dispatch({ type: "GET_FILTER_VMWORKLOADTYPE", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
