import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { ResourceKeyMasterApi } from "../../../Business/ResourceKeyMasterBusiness";
import {
  GET_FILTER_NETWORK_ELEMENT_AS_IS,
  GET_GRID_RESOURCE_KEY_MASTER,
  NetworkElementAsIsGrid,
  ResourceKeyMasterGrid,
  ResourceKeyMasterQueryObjectGrid,
  QueryResultDtoOfResourceKeyMasterDtoGrid,
} from "../../../Model/ResourceKeyMaster";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetResourceKeyMasterGrid(
  queryFilter?: ResourceKeyMasterQueryObjectGrid
) {
  setLoader("ADD", "GetResourceKeyMasterGrid");
  let api = new ResourceKeyMasterApi();

  let result: QueryResultDtoOfResourceKeyMasterDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfResourceKeyMasterDtoGrid>
    >(() => api.resourceKeyMasterGetResourceKeyMaster(queryFilter ?? {}));

    let rtn = {
      ResourceKeyMasterGridResult: result,
      filter: null,
    } as ResourceKeyMasterGrid;

    rootStore.dispatch({ type: GET_GRID_RESOURCE_KEY_MASTER, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_RESOURCE_KEY_MASTER,
      payload: {
        ResourceKeyMasterGridResult: null,
        filter: null,
      } as ResourceKeyMasterGrid,
    });
  }
  setLoader("REMOVE", "GetResourceKeyMasterGrid");
}

export async function GetFilterColumResourceKeyMaster(
  columName: string,
  columValue: string,
  queryFilter?: ResourceKeyMasterQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new ResourceKeyMasterApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.resourceKeyMasterGetFilterResult(
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
