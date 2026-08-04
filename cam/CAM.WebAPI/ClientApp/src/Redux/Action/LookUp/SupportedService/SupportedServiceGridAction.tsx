import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { SupportedServiceApi } from "../../../../Business/LookUp/SupportedServiceBusiness";
import {
  SupportedServiceQueryObjectGrid,
  QueryResultDtoOfSupportedServiceDtoGrid,
} from "../../../../Model/LookUp/SupportedService";
import {
  TipologicaGridDto,
  QueryResultDtoOfTipologicaGridDto,
  LookUpGrid,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetSupportedServiceGrid(
  queryFilter?: SupportedServiceQueryObjectGrid
) {
  setLoader("ADD", "GetSupportedServiceGrid");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new SupportedServiceApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfSupportedServiceDtoGrid>
      >(() =>
        api.supportedServiceGetSupportedService(
          queryFilter?.id,
          queryFilter?.description,
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
        Promise<QueryResultDtoOfSupportedServiceDtoGrid>
      >(() => api.supportedServiceGetSupportedService());
    }
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_SUPPORTED_SERVICE",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SUPPORTED_SERVICE",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetSupportedServiceGrid");
}

export async function GetSupportedServiceGridALL() {
  setLoader("ADD", "GetSupportedServiceGridALL");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new SupportedServiceApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfSupportedServiceDtoGrid>
    >(() => api.supportedServiceGetSupportedService());
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_SUPPORTED_SERVICE_ALL",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_SUPPORTED_SERVICE_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetSupportedServiceGridALL");
}

export async function GetFilterColumSupportedService(
  columName: string,
  columValue: string,
  queryFilter?: SupportedServiceQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumSupportedService");

  let result: FilterValueDto[] | undefined;
  let api = new SupportedServiceApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.supportedServiceGetFilterResult(
        columName,
        columValue,
        queryFilter?.id,
        queryFilter?.description,
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
      api.supportedServiceGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({ type: "GET_FILTER_SUPPORTED_SERVICE", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumSupportedService");
}
