import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { NetworkElementAsPlannedApi } from "../../../Business/NetworkElementAsPlannedBusiness";
import {
  GET_FILTER_NETWORK_ELEMENT_AS_PLANNED,
  GET_GRID_NETWORK_ELEMENT_AS_PLANNED,
  NetworkElementAsPlannedGrid,
  NetworkElementAsPlannedQueryObjectGrid,
  QueryResultDtoOfNetworkElementAsPlannedDtoGrid,
} from "../../../Model/NetworkElementAsPlanned";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetNetworkElementAsPlannedGrid(
  queryFilter?: NetworkElementAsPlannedQueryObjectGrid
) {
  setLoader("ADD", "GetNetworkElementAsPlannedGrid");

  let api = new NetworkElementAsPlannedApi();
  let result: QueryResultDtoOfNetworkElementAsPlannedDtoGrid | null | undefined;
  try {
    console.log("node index", queryFilter?.nodeIndex);
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfNetworkElementAsPlannedDtoGrid>
    >(() => api.networkElementAsPlannedGetLcmengineering(queryFilter ?? {}));

    let rtn = {
      NetworkElementAsPlannedGridResult: result,
      filter: null,
    } as NetworkElementAsPlannedGrid;
    rootStore.dispatch({
      type: GET_GRID_NETWORK_ELEMENT_AS_PLANNED,
      payload: rtn,
    });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_NETWORK_ELEMENT_AS_PLANNED,
      payload: {
        NetworkElementAsPlannedGridResult: null,
        filter: null,
      } as NetworkElementAsPlannedGrid,
    });
  }
  setLoader("REMOVE", "GetNetworkElementAsPlannedGrid");
}

export async function GetFilterColumNetworkElementAsPlanned(
  columName: string,
  columValue: string,
  queryFilter?: NetworkElementAsPlannedQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumNetworkElementAsPlanned");

  let result: FilterValueDto[] | undefined;
  let api = new NetworkElementAsPlannedApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.networkElementAsPlannedGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );

  let rtn = {
    filter: result,
    NetworkElementAsPlannedGridResult: null,
  } as NetworkElementAsPlannedGrid;
  rootStore.dispatch({
    type: GET_FILTER_NETWORK_ELEMENT_AS_PLANNED,
    payload: rtn,
  });
  // setLoader("REMOVE", "GetFilterColumNetworkElementAsPlanned");
}
