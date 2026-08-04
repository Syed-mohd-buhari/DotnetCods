import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { ServiceBoundaryApi } from "../../../../Business/LookUp/ServiceBoundaryBusiness";
import {
  QueryResultDtoOfServiceBoundaryGridDto,
  ServiceBoundaryQueryDto,
  LookUpServiceBoundaryGrid,
} from "../../../../Model/LookUp/ServiceBoundary";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetServiceBoundaryGrid(
  queryFilter?: ServiceBoundaryQueryDto
) {
  setLoader("ADD", "GetServiceBoundaryGrid");
  let result: QueryResultDtoOfServiceBoundaryGridDto | null | undefined;
  let api = new ServiceBoundaryApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfServiceBoundaryGridDto>
      >(() =>
        api.serviceBoundaryGetServiceBoundary(
          queryFilter?.serviceBoundaryId,
          queryFilter?.serviceBoundaryDescription,
          queryFilter?.sortBy,
          queryFilter?.isSortAscending,
          queryFilter?.page,
          queryFilter?.pageSize,
          queryFilter?.lastModifiedStartDate,
          queryFilter?.lastModifiedEndDate,
          queryFilter?.principalId,
          queryFilter?.deleted,
          queryFilter?.orphan,
          queryFilter?.lastModifiedBy
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfServiceBoundaryGridDto>
      >(() => api.serviceBoundaryGetServiceBoundary());
    }
    // if (result?.items?.length === 0 || result?.totalItems === undefined) {
    //     rootStore.dispatch(setNotification({ message: "no results found", notifyType: NotifyType.warning }));
    // }
    let rtn = {
      LookUpGridResult: result,
      filter: null,
    } as LookUpServiceBoundaryGrid;
    rootStore.dispatch({
      type: "GET_GRID_SERVICE_BOUNDARY",
      payload: rtn as LookUpServiceBoundaryGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SERVICE_BOUNDARY",
      payload: {
        LookUpGridResult: result,
        filter: null,
      } as LookUpServiceBoundaryGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetServiceBoundaryGrid");
}

export async function GetServiceBoundaryGridALL() {
  setLoader("ADD", "GetServiceBoundaryGridALL");

  let result: QueryResultDtoOfServiceBoundaryGridDto | null | undefined;
  let api = new ServiceBoundaryApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfServiceBoundaryGridDto>
    >(() => api.serviceBoundaryGetServiceBoundary());
    let rtn = {
      LookUpGridResult: result,
      filter: null,
    } as LookUpServiceBoundaryGrid;
    rootStore.dispatch({
      type: "GET_GRID_SERVICE_BOUNDARY_ALL",
      payload: rtn as LookUpServiceBoundaryGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SERVICE_BOUNDARY_ALL",
      payload: {
        LookUpGridResult: result,
        filter: null,
      } as LookUpServiceBoundaryGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetServiceBoundaryGridALL");
}

export async function GetFilterColumServiceBoundary(
  columName: string,
  columValue: string,
  queryFilter?: ServiceBoundaryQueryDto
) {
  // setLoader("ADD", "GetFilterColumServiceBoundary");

  let result: FilterValueDto[] | undefined;
  let api = new ServiceBoundaryApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.serviceBoundaryGetFilterResult(
        columName,
        columValue,
        queryFilter?.serviceBoundaryId,
        queryFilter?.serviceBoundaryDescription,
        queryFilter?.sortBy,
        queryFilter?.isSortAscending,
        queryFilter?.page,
        queryFilter?.pageSize,
        queryFilter?.lastModifiedStartDate,
        queryFilter?.lastModifiedEndDate,
        queryFilter?.principalId,
        queryFilter?.deleted,
        queryFilter?.orphan,
        queryFilter?.lastModifiedBy
      )
    );
  } else {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.serviceBoundaryGetFilterResult(columName, columValue)
    );
  }
  let rtn = {
    filter: result,
    LookUpGridResult: null,
  } as LookUpServiceBoundaryGrid;
  rootStore.dispatch({ type: "GET_FILTER_SERVICE_BOUNDARY", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumServiceBoundary");
}
