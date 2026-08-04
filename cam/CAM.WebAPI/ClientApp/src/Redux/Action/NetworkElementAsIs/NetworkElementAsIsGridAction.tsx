import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { NetworkElementAsIsApi } from "../../../Business/NetworkElementAsIsBusiness";
import {
  GET_FILTER_NETWORK_ELEMENT_AS_IS,
  GET_GRID_NETWORK_ELEMENT_AS_IS,
  NetworkElementAsIsGrid,
  NewNetworkElementAsIsGrid,
  NetworkElementAsIsQueryObjectGrid,
  QueryResultDtoOfNetworkElementAsIsDtoGrid,
} from "../../../Model/NetworkElementAsIs";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetNetworkElementAsIsGrid(
  queryFilter?: NetworkElementAsIsQueryObjectGrid
) {
  setLoader("ADD", "GetNetworkElementAsIsGrid");

  let api = new NetworkElementAsIsApi();

  let result: QueryResultDtoOfNetworkElementAsIsDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfNetworkElementAsIsDtoGrid>
    >(() => api.networkElementAsIsGetNetworkElementAsIs(queryFilter ?? {}));

    let rtn = {
      NetworkElementAsIsGridResult: result,
      filter: null,
    } as NetworkElementAsIsGrid;

    rootStore.dispatch({ type: GET_GRID_NETWORK_ELEMENT_AS_IS, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_NETWORK_ELEMENT_AS_IS,
      payload: {
        NetworkElementAsIsGridResult: null,
        filter: null,
      } as NetworkElementAsIsGrid,
    });
  }
  setLoader("REMOVE", "GetNetworkElementAsIsGrid");
}

//Added as part of Tems
export async function GetNewNetworkElementAsIsGrid(
  queryFilter?: NetworkElementAsIsQueryObjectGrid
) {
  setLoader("ADD", "GetNetworkElementAsIsGrid");
  let api = new NetworkElementAsIsApi();

  let result: QueryResultDtoOfNetworkElementAsIsDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfNetworkElementAsIsDtoGrid>
    >(() => api.newnetworkElementAsIsGetNetworkElementAsIs(queryFilter ?? {}));

    let rtn = {
      NewNetworkElementAsIsGridResult: result,
      filter: null,
    } as NewNetworkElementAsIsGrid;

    rootStore.dispatch({ type: GET_GRID_NETWORK_ELEMENT_AS_IS, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_NETWORK_ELEMENT_AS_IS,
      payload: {
        NewNetworkElementAsIsGridResult: null,
        filter: null,
      } as NewNetworkElementAsIsGrid,
    });
  }
  setLoader("REMOVE", "GetNetworkElementAsIsGrid");
}

export async function GetFilterColumNetworkElementAsIs(
  columName: string,
  columValue: string,
  queryFilter?: NetworkElementAsIsQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new NetworkElementAsIsApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.networkElementAsIsGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    NetworkElementAsIsGridResult: null,
  } as NetworkElementAsIsGrid;
  rootStore.dispatch({ type: GET_FILTER_NETWORK_ELEMENT_AS_IS, payload: rtn });
}

export async function GetFilterColumNewNetworkElementAsIs(
  columName: string,
  columValue: string,
  queryFilter?: NetworkElementAsIsQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new NetworkElementAsIsApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.newnetworkElementAsIsGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    NetworkElementAsIsGridResult: null,
  } as NetworkElementAsIsGrid;
  rootStore.dispatch({ type: GET_FILTER_NETWORK_ELEMENT_AS_IS, payload: rtn });
}
