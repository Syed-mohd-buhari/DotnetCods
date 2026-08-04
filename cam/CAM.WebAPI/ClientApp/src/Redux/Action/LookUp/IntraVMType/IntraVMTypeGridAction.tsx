import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { IntraVMTypeApi } from "../../../../Business/LookUp/IntraVMTypeBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  IntraVMTypeGrid,
  IntraVMTypeQueryObjectGrid,
  QueryResultDtoOfIntraVMTypeDtoGrid,
} from "../../../../Model/LookUp/IntraVMType";

export async function GetIntraVMTypeGrid(
  queryFilter?: IntraVMTypeQueryObjectGrid
) {
  setLoader("ADD", "GetIntraVMTypeGrid");

  let result: QueryResultDtoOfIntraVMTypeDtoGrid | null | undefined;
  let api = new IntraVMTypeApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfIntraVMTypeDtoGrid>
      >(() => api.IntraVMTypeGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfIntraVMTypeDtoGrid>
      >(() => api.IntraVMTypeGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as IntraVMTypeGrid;
    rootStore.dispatch({
      type: "GET_GRID_INTRAVMTYPE",
      payload: rtn as IntraVMTypeGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_INTRAVMTYPE",
      payload: { LookUpGridResult: result, filter: null } as IntraVMTypeGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetIntraVMTypeGrid");
}

export async function GetIntraVMTypeGridALL(queryFilter) {
  setLoader("ADD", "GetIntraVMTypeGridALL");

  let result: QueryResultDtoOfIntraVMTypeDtoGrid | null | undefined;
  let api = new IntraVMTypeApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfIntraVMTypeDtoGrid>
    >(() => api.IntraVMTypeGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as IntraVMTypeGrid;
    rootStore.dispatch({
      type: "GET_GRID_INTRAVMTYPE_ALL",
      payload: rtn as IntraVMTypeGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_INTRAVMTYPE_ALL",
      payload: { LookUpGridResult: result, filter: null } as IntraVMTypeGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetIntraVMTypeGridALL");
}

export async function GetFilterColumIntraVMType(
  columName: string,
  columValue: string,
  queryFilter?: IntraVMTypeQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new IntraVMTypeApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.IntraVMTypeGetFilterResult(
        columName,
        columValue,
        queryFilter?.intraDescription,
        queryFilter?.intraVmTypeId,
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
      api.IntraVMTypeGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as IntraVMTypeGrid;
  rootStore.dispatch({ type: "GET_FILTER_INTRAVMTYPE", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
