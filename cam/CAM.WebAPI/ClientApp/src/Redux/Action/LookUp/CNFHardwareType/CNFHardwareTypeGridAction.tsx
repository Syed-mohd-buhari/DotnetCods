import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { CNFHardwareTypeApi } from "../../../../Business/LookUp/CNFHardwareTypeBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  CNFHardwareTypeGrid,
  CNFHardwareTypeQueryObjectGrid,
  QueryResultDtoOfCNFHardwareTypeDtoGrid,
} from "../../../../Model/LookUp/CNFHardwareType";

export async function GetCNFHardwareTypeGrid(
  queryFilter?: CNFHardwareTypeQueryObjectGrid
) {
  setLoader("ADD", "GetLocationGrid");

  let result: QueryResultDtoOfCNFHardwareTypeDtoGrid | null | undefined;
  let api = new CNFHardwareTypeApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfCNFHardwareTypeDtoGrid>
      >(() => api.CNFHardwareTypeGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfCNFHardwareTypeDtoGrid>
      >(() => api.CNFHardwareTypeGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as CNFHardwareTypeGrid;
    rootStore.dispatch({
      type: "GET_GRID_CNFHARDWARETYPE",
      payload: rtn as CNFHardwareTypeGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_CNFHARDWARETYPE",
      payload: {
        LookUpGridResult: result,
        filter: null,
      } as CNFHardwareTypeGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetCNFHardwareTypeGrid");
}

export async function GetCNFHardwareTypeGridALL(queryFilter) {
  setLoader("ADD", "GetCNFHardwareTypeGridALL");

  let result: QueryResultDtoOfCNFHardwareTypeDtoGrid | null | undefined;
  let api = new CNFHardwareTypeApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfCNFHardwareTypeDtoGrid>
    >(() => api.CNFHardwareTypeGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as CNFHardwareTypeGrid;
    rootStore.dispatch({
      type: "GET_GRID_CNFHARDWARETYPE_ALL",
      payload: rtn as CNFHardwareTypeGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_CNFHARDWARETYPE_ALL",
      payload: {
        LookUpGridResult: result,
        filter: null,
      } as CNFHardwareTypeGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetCNFHardwareTypeGridALL");
}

export async function GetFilterColumCNFHardwareType(
  columName: string,
  columValue: string,
  queryFilter?: CNFHardwareTypeQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new CNFHardwareTypeApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.CNFHardwareTypeGetFilterResult(
        columName,
        columValue,
        queryFilter?.description,
        queryFilter?.cnfHardwareId,
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
      api.CNFHardwareTypeGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as CNFHardwareTypeGrid;
  rootStore.dispatch({ type: "GET_FILTER_CNFHARDWARETYPE", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
