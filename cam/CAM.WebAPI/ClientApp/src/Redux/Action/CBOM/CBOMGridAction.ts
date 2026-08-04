import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { CBOMApi } from "../../../Business/CBOMBusiness";
import {
  GET_FILTER_CBOM,
  GET_GRID_CBOM,
  CBOMGrid,
  CBOMQueryObjectGrid,
  QueryResultDtoOfCBOMDtoGrid,
  CBOMClusterInfoQueryObjectGrid,
  QueryResultDtoOfCBOMClusterInfoDtoGrid,
  CBOMClusterInfoGrid,
  GET_GRID_CBOM_CLUSTER_INFO,
  GET_GRID_CBOM_CNF_INSTANCE_AND_CAPACITY,
  CBOMCnfInstanceAndCapacityGrid,
  GET_FILTER_CBOM_CNF_INSTANCE_AND_CAPACITY,
  GET_FILTER_CBOM_CLUSTER_INFO,
  GET_FILTER_CBOM_CNF_CAPACITY,
} from "../../../Model/CBOM";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import { QueryResultDtoOfVBOMVnfInfoAndCapacityDtoGrid } from "../../../Model/VBOMInfo";

export async function GetCBOMGrid(
  queryFilter?: CBOMQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetCBOMGrid");

  let result: QueryResultDtoOfCBOMDtoGrid | null | undefined;
  let api = new CBOMApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfCBOMDtoGrid>
    >(() => api.CBOMGetGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_CBOM,
        payload: {
          CBOMGridResult: result,
          filter: null,
        } as CBOMGrid,
      });
    } else {
      setLoader("REMOVE", "GetCBOMGrid");

      return result?.items;
    }
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_CBOM,
      payload: {
        CBOMGridResult: null,
        filter: null,
      } as CBOMGrid,
    });
  }
  setLoader("REMOVE", "GetCBOMGrid");
}

export async function GetCBOMClusterInfoGrid(
  queryFilter?: CBOMClusterInfoQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetCBOMClusterInfoGrid");

  let result: QueryResultDtoOfCBOMClusterInfoDtoGrid | null | undefined;
  let api = new CBOMApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfCBOMClusterInfoDtoGrid>
    >(() => api.CBOMGetGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_CBOM_CLUSTER_INFO,
        payload: {
          CBOMClusterInfoGridResult: result,
          filter: null,
        } as CBOMClusterInfoGrid,
      });
    } else {
      setLoader("REMOVE", "GetCBOMClusterInfoGrid");

      return result?.items;
    }
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_CBOM_CLUSTER_INFO,
      payload: {
        CBOMClusterInfoGridResult: null,
        filter: null,
      } as CBOMClusterInfoGrid,
    });
  }
  setLoader("REMOVE", "GetCBOMClusterInfoGrid");
}

export async function GetCnfInstanceAndCapacityDetails(
  queryFilter?: CBOMClusterInfoQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetCnfInstanceAndCapacityDetails");

  let result: QueryResultDtoOfVBOMVnfInfoAndCapacityDtoGrid | null | undefined;
  let api = new CBOMApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfVBOMVnfInfoAndCapacityDtoGrid>
    >(() => api.CBOMCnfInstanceAndCapacityGetGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_CBOM_CNF_INSTANCE_AND_CAPACITY,
        payload: {
          CBOMCnfInstanceAndCapacityGridResult: result,
          filter: null,
        } as CBOMCnfInstanceAndCapacityGrid,
      });
      setLoader("REMOVE", "GetCnfInstanceAndCapacityDetails");
      return result;
    } else {
      setLoader("REMOVE", "GetCnfInstanceAndCapacityDetails");
      return result;
    }
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_CBOM_CNF_INSTANCE_AND_CAPACITY,
      payload: {
        CBOMCnfInstanceAndCapacityGridResult: null,
        filter: null,
      } as CBOMCnfInstanceAndCapacityGrid,
    });
  }
  setLoader("REMOVE", "GetCnfInstanceAndCapacityDetails");
}

export async function GetFilterColumnCBOM(
  columName: string,
  columValue: string,
  queryFilter?: CBOMQueryObjectGrid
) {
  let api = new CBOMApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.CBOMGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    CBOMGridResult: null,
  } as CBOMGrid;
  rootStore.dispatch({ type: GET_FILTER_CBOM, payload: rtn });

  return rtn;
}

export async function GetFilterColumnCBOMClusterInfo(
  columName: string,
  columValue: string,
  queryFilter?: CBOMQueryObjectGrid
) {
  let api = new CBOMApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.CBOMGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    CBOMGridResult: null,
  } as CBOMGrid;
  rootStore.dispatch({ type: GET_FILTER_CBOM_CLUSTER_INFO, payload: rtn });

  return rtn;
}

export async function GetFilterColumnCBOMInstanceCapacity(
  columName: string,
  columValue: string,
  queryFilter?: CBOMQueryObjectGrid,
  isInstance?: boolean
) {
  let api = new CBOMApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.CBOMGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue,
      (isInstance = true)
    )
  );
  let rtn = {
    filter: result,
    CBOMGridResult: null,
  } as CBOMGrid;
  rootStore.dispatch({
    type: GET_FILTER_CBOM_CNF_INSTANCE_AND_CAPACITY,
    payload: rtn,
  });

  return rtn;
}
export async function GetFilterColumnCBOMCnfCapacityFilter(
  columName: string,
  columValue: string,
  queryFilter?: CBOMQueryObjectGrid,
  isInstance?: boolean
) {
  let api = new CBOMApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.CBOMCnfCapacityFilter(
      queryFilter ?? {},
      columName,
      columValue,
      (isInstance = true)
    )
  );
  let rtn = {
    filter: result,
    CBOMGridResult: null,
  } as CBOMGrid;
  rootStore.dispatch({
    type: GET_FILTER_CBOM_CNF_CAPACITY,
    payload: rtn,
  });

  return rtn;
}
