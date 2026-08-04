import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { ServiceMasterApi } from "../../../../Business/LookUp/ServiceMasterBusiness";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";
import {
  ServiceMasterGrid,
  ServiceMasterQueryObjectGrid,
  QueryResultDtoOfServiceMasterDtoGrid,
} from "../../../../Model/LookUp/ServiceMaster";

export async function GetServiceMasterGrid(
  queryFilter?: ServiceMasterQueryObjectGrid
) {
  setLoader("ADD", "GetServiceMasterGrid");

  let result: QueryResultDtoOfServiceMasterDtoGrid | null | undefined;
  let api = new ServiceMasterApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfServiceMasterDtoGrid>
      >(() => api.ServiceMasterGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfServiceMasterDtoGrid>
      >(() => api.ServiceMasterGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as ServiceMasterGrid;
    rootStore.dispatch({
      type: "GET_GRID_SERVICEMASTER",
      payload: rtn as ServiceMasterGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SERVICEMASTER",
      payload: { LookUpGridResult: result, filter: null } as ServiceMasterGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetServiceMasterGrid");
}

export async function GetServiceMasterGridALL(queryFilter) {
  setLoader("ADD", "GetServiceMasterGridALL");

  let result: QueryResultDtoOfServiceMasterDtoGrid | null | undefined;
  let api = new ServiceMasterApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfServiceMasterDtoGrid>
    >(() => api.ServiceMasterGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as ServiceMasterGrid;
    rootStore.dispatch({
      type: "GET_GRID_SERVICEMASTER_ALL",
      payload: rtn as ServiceMasterGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SERVICEMASTER_ALL",
      payload: { LookUpGridResult: result, filter: null } as ServiceMasterGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetServiceMasterGridALL");
}

export async function GetFilterColumServiceMaster(
  columName: string,
  columValue: string,
  queryFilter?: ServiceMasterQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new ServiceMasterApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.ServiceMasterGetFilterResult(
        columName,
        columValue,
        queryFilter?.description,
        queryFilter?.serviceMasterIndex,
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
      api.ServiceMasterGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as ServiceMasterGrid;
  rootStore.dispatch({ type: "GET_FILTER_SERVICEMASTER", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
