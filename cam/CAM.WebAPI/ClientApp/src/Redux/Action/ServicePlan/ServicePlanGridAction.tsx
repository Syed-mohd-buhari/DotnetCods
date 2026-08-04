import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { ServicePlanApi } from "../../../Business/ServicePlanBusiness";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import {
  ServicePlanGrid,
  ServicePlanQueryObjectGrid,
  QueryResultDtoOfServicePlanDtoGrid,
} from "../../../Model/ServicePlan";

export async function GetServicePlanGrid(
  queryFilter?: ServicePlanQueryObjectGrid
) {
  setLoader("ADD", "GetServicePlanGrid");

  let result: QueryResultDtoOfServicePlanDtoGrid | null | undefined;
  let api = new ServicePlanApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfServicePlanDtoGrid>
      >(() => api.ServicePlanGet(queryFilter));
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfServicePlanDtoGrid>
      >(() => api.ServicePlanGet());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = { LookUpGridResult: result, filter: null } as ServicePlanGrid;
    rootStore.dispatch({
      type: "GET_GRID_SERVICEPLAN",
      payload: rtn as ServicePlanGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SERVICEPLAN",
      payload: { LookUpGridResult: result, filter: null } as ServicePlanGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetServicePlanGrid");
}

export async function GetServicePlanGridALL(queryFilter) {
  setLoader("ADD", "GetServicePlanGridALL");

  let result: QueryResultDtoOfServicePlanDtoGrid | null | undefined;
  let api = new ServicePlanApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfServicePlanDtoGrid>
    >(() => api.ServicePlanGet(queryFilter));
    let rtn = { LookUpGridResult: result, filter: null } as ServicePlanGrid;
    rootStore.dispatch({
      type: "GET_GRID_SERVICEPLAN_ALL",
      payload: rtn as ServicePlanGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SERVICEPLAN_ALL",
      payload: { LookUpGridResult: result, filter: null } as ServicePlanGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetServicePlanGridALL");
}

export async function GetFilterColumServicePlan(
  columName: string,
  columValue: string,
  queryFilter?: ServicePlanQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumLocation");

  let result: FilterValueDto[] | undefined;
  let api = new ServicePlanApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.ServicePlanGetFilterResult(
        columName,
        columValue,
        queryFilter?.program,
        queryFilter?.servicePlanId,
        queryFilter?.serviceMasterId,
        queryFilter?.opCoId,
        queryFilter?.dcfId,
        queryFilter?.status,
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
      api.ServicePlanGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as ServicePlanGrid;
  rootStore.dispatch({ type: "GET_FILTER_SERVICEPLAN", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumLocation");
}
