import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { DCFLifeCycleApi } from "../../../Business/DCFLifeCycleBusiness";
import {
  GET_FILTER_NETWORK_ELEMENT_AS_IS,
  GET_GRID_RESOURCE_KEY_MASTER,
  NetworkElementAsIsGrid,
  DCFLifeCycleGrid,
  DCFLifeCycleQueryObjectGrid,
  QueryResultDtoOfDCFLifeCycleDtoGrid,
} from "../../../Model/DCFLifeCycle";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetDCFLifeCycleGrid(
  queryFilter?: DCFLifeCycleQueryObjectGrid
) {
  setLoader("ADD", "GetDCFLifeCycleGrid");
  let api = new DCFLifeCycleApi();

  let result: QueryResultDtoOfDCFLifeCycleDtoGrid | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfDCFLifeCycleDtoGrid>
    >(() => api.dcfLifeCycleGetDCFLifeCycle(queryFilter ?? {}));

    let rtn = {
      DCFLifeCycleGridResult: result,
      filter: null,
    } as DCFLifeCycleGrid;

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
        DCFLifeCycleGridResult: null,
        filter: null,
      } as DCFLifeCycleGrid,
    });
  }
  setLoader("REMOVE", "GetDCFLifeCycleGrid");
}

export async function GetFilterColumDCFLifeCycle(
  columName: string,
  columValue: string,
  queryFilter?: DCFLifeCycleQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new DCFLifeCycleApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.dcfLifeCycleGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    NetworkElementAsIsGridResult: null,
  } as NetworkElementAsIsGrid;
  rootStore.dispatch({ type: GET_FILTER_NETWORK_ELEMENT_AS_IS, payload: rtn });
}
