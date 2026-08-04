import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../../Business/Common/CommonBusiness";
import { NetworkFunctionApi } from "../../../../Business/LookUp/NetworkFunctionBusiness";
import {
  NetworkFunctionQueryObjectGrid,
  QueryResultDtoOfNetworkFunctionDtoGrid,
} from "../../../../Model/LookUp/NetworkFunction";
import {
  TipologicaGridDto,
  QueryResultDtoOfTipologicaGridDto,
  LookUpGrid,
} from "../../../../Model/LookUp/LookUpGenericModel";
import { NotifyType } from "../../../Reducer/NotificationReducer";
import { rootStore } from "../../../Store/rootStore";
import setLoader from "../../LoaderAction";
import { setNotification } from "../../NotificationAction";

export async function GetNetworkFunctionGrid(
  queryFilter?: NetworkFunctionQueryObjectGrid
) {
  setLoader("ADD", "GetNetworkFunctionGrid");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new NetworkFunctionApi();
  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfNetworkFunctionDtoGrid>
      >(() =>
        api.networkFunctionGetNetworkFunction(
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
        Promise<QueryResultDtoOfNetworkFunctionDtoGrid>
      >(() => api.networkFunctionGetNetworkFunction());
    }
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_NETWORK_FUNCTION",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_NETWORK_FUNCTION",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetNetworkFunctionGrid");
}

export async function GetNetworkFunctionGridALL() {
  setLoader("ADD", "GetNetworkFunctionGridALL");

  let result: QueryResultDtoOfTipologicaGridDto | null | undefined;
  let api = new NetworkFunctionApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfNetworkFunctionDtoGrid>
    >(() => api.networkFunctionGetNetworkFunction());
    let rtn = { LookUpGridResult: result, filter: null } as TipologicaGridDto;
    rootStore.dispatch({
      type: "GET_GRID_NETWORK_FUNCTION_ALL",
      payload: rtn as TipologicaGridDto,
    });
  } catch (error) {
    rootStore.dispatch({
      type: "GET_GRID_NETWORK_FUNCTION_ALL",
      payload: { LookUpGridResult: result, filter: null } as LookUpGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetNetworkFunctionGridALL");
}

export async function GetFilterColumNetworkFunction(
  columName: string,
  columValue: string,
  queryFilter?: NetworkFunctionQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumNetworkFunction");

  let result: FilterValueDto[] | undefined;
  let api = new NetworkFunctionApi();
  if (queryFilter !== null && queryFilter !== undefined) {
    result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
      api.networkFunctionGetFilterResult(
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
      api.networkFunctionGetFilterResult(columName, columValue)
    );
  }
  let rtn = { filter: result, LookUpGridResult: null } as LookUpGrid;
  rootStore.dispatch({ type: "GET_FILTER_NETWORK_FUNCTION", payload: rtn });
  // setLoader("REMOVE", "GetFilterColumNetworkFunction");
}
