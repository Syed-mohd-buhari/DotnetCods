import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { InterVMTypeApi } from "../../../../Business/LookUp/InterVMTypeBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  InterVMTypeGrid,
  InterVMTypeQueryObjectGrid,
  QueryResultDtoOfInterVMTypeDtoGrid,
} from "../../../../Model/LookUp/InterVMType";

export async function GetInterVMTypeGrid(
  queryFilter?: InterVMTypeQueryObjectGrid
) {
  setLoader("ADD", "GetLocationGrid");

  let result: QueryResultDtoOfInterVMTypeDtoGrid | null | undefined;
  let api = new InterVMTypeApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfInterVMTypeDtoGrid>
      >(() => api.InterVMTypeGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfInterVMTypeDtoGrid>
      >(() => api.InterVMTypeGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as InterVMTypeGrid;
    rootStore.dispatch({
      type: "GET_GRID_INTERVMTYPE",
      payload: rtn as InterVMTypeGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_INTERVMTYPE",
      payload: { LookUpGridResult: result, filter: null } as InterVMTypeGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetInterVMTypeGrid");
}

export async function GetInterVMTypeGridALL(queryFilter) {
  setLoader("ADD", "GetInterVMTypeGridALL");

  let result: QueryResultDtoOfInterVMTypeDtoGrid | null | undefined;
  let api = new InterVMTypeApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfInterVMTypeDtoGrid>
    >(() => api.InterVMTypeGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as InterVMTypeGrid;
    rootStore.dispatch({
      type: "GET_GRID_INTERVMTYPE_ALL",
      payload: rtn as InterVMTypeGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_INTERVMTYPE_ALL",
      payload: { LookUpGridResult: result, filter: null } as InterVMTypeGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetInterVMTypeGridALL");
}

export async function GetFilterColumInterVMType(
  columName: string,
  columValue: string,
  queryFilter?: InterVMTypeQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new InterVMTypeApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.InterVMTypeGetFilterResult(
        columName,
        columValue,
        queryFilter?.interDescription,
        queryFilter?.interVmTypeId,
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
      api.InterVMTypeGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as InterVMTypeGrid;
  rootStore.dispatch({ type: "GET_FILTER_INTERVMTYPE", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
