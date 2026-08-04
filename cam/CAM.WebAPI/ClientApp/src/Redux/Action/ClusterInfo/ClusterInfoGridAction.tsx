import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";

import { ClusterInfoApi } from "../../../Business/ClusterInfoBusiness";
import {
  LookUpGrid,
  QueryResultDtoOfTipologicaGridDto,
  TipologicaGridDto,
  TipologicheQueryObjectGrid,
} from "../../../Model/LookUp/LookUpGenericModel";
import {
  GET_FILTER_CLUSTER_INFO,
  GET_GRID_CLUSTER_INFO,
  QueryResultDtoOfClusterInfoDtoGrid,
  ClusterInfoGrid,
  ClusterInfoQueryObjectGrid,
} from "../../../Model/ClusterInfo";

import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetClusterInfoGrid(
  queryFilter?: ClusterInfoQueryObjectGrid
) {
  setLoader("ADD", "GetClusterInfoGrid");
  let result: QueryResultDtoOfClusterInfoDtoGrid | null | undefined;
  let api = new ClusterInfoApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfClusterInfoDtoGrid>
    >(() => api.ClusterInfoGetGrid(queryFilter ?? {}));

    let rtn = {
      ClusterInfoGridResult: result,
      filter: null,
    } as ClusterInfoGrid;
    rootStore.dispatch({
      type: GET_GRID_CLUSTER_INFO,
      payload: rtn,
    });

    return result;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_CLUSTER_INFO,
      payload: {
        ClusterInfoGridResult: null,
        filter: null,
      } as ClusterInfoGrid,
    });
  }
  setLoader("REMOVE", "GetClusterInfoGrid");
}

export async function GetFilterColumClusterInfo(
  columName: string,
  columValue: string,
  queryFilter?: ClusterInfoQueryObjectGrid
) {
  let api = new ClusterInfoApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.ClusterInfoGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    ClusterInfoGridResult: null,
  } as ClusterInfoGrid;
  rootStore.dispatch({ type: GET_FILTER_CLUSTER_INFO, payload: rtn });

  return rtn;
}
