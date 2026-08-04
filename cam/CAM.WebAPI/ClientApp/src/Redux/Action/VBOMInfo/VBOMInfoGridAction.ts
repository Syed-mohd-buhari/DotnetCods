import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { VBOMInfoApi } from "../../../Business/VBOMInfoBusiness";
import {
  GET_FILTER_VBOM_INFO,
  GET_GRID_VBOM_INFO,
  VBOMInfoGrid,
  VBOMInfoQueryObjectGrid,
  QueryResultDtoOfVBOMInfoDtoGrid,
  QueryResultDtoOfVBOMClusterInfoDtoGrid,
  VBOMClusterInfoQueryObjectGrid,
  VBOMClusterInfoGrid,
  GET_GRID_VBOM_CLUSTER_INFO,
  QueryResultDtoOfVBOMVnfInfoAndCapacityDtoGrid,
  GET_GRID_VBOM_VNF_INSTANCE_AND_CAPACITY,
  VBOMVnfInstanceAndCapacityGrid,
  GET_FILTER_VBOM_CLUSTER_INFO,
  GET_FILTER_VBOM_VNF_INSTANCE_AND_CAPACITY,
  GET_FILTER_VBOM_VNF_CAPACITY,
} from "../../../Model/VBOMInfo";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetVBOMInfoGrid(
  queryFilter?: VBOMInfoQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetVBOMInfoGrid");

  let result: QueryResultDtoOfVBOMInfoDtoGrid | null | undefined;
  let api = new VBOMInfoApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfVBOMInfoDtoGrid>
    >(() => api.VBOMInfoGetGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_VBOM_INFO,
        payload: {
          VBOMInfoGridResult: result,
          filter: null,
        } as VBOMInfoGrid,
      });
    } else {
      setLoader("REMOVE", "GetVBOMInfoGrid");

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
      type: GET_GRID_VBOM_INFO,
      payload: {
        VBOMInfoGridResult: null,
        filter: null,
      } as VBOMInfoGrid,
    });
  }
  setLoader("REMOVE", "GetVBOMInfoGrid");
}

export async function GetVBOMClusterInfoGrid(
  queryFilter?: VBOMClusterInfoQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetVBOMClusterInfoGrid");

  let result: QueryResultDtoOfVBOMClusterInfoDtoGrid | null | undefined;
  let api = new VBOMInfoApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfVBOMClusterInfoDtoGrid>
    >(() => api.VBOMClusterInfoGetGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_VBOM_CLUSTER_INFO,
        payload: {
          VBOMClusterInfoGridResult: result,
          filter: null,
        } as VBOMClusterInfoGrid,
      });
    } else {
      setLoader("REMOVE", "GetVBOMClusterInfoGrid");

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
      type: GET_GRID_VBOM_CLUSTER_INFO,
      payload: {
        VBOMClusterInfoGridResult: null,
        filter: null,
      } as VBOMClusterInfoGrid,
    });
  }
  setLoader("REMOVE", "GetVBOMClusterInfoGrid");
}

export async function GetVnfInfoAndCapacityDetails(
  queryFilter?: VBOMClusterInfoQueryObjectGrid,
  returnValues?: boolean
) {
  setLoader("ADD", "GetVnfInfoAndCapacityDetails");

  let result: QueryResultDtoOfVBOMVnfInfoAndCapacityDtoGrid | null | undefined;
  let api = new VBOMInfoApi();
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfVBOMVnfInfoAndCapacityDtoGrid>
    >(() => api.VBOMVnfInfoAndCapacityGetGrid(queryFilter ?? {}));

    if (returnValues != true) {
      rootStore.dispatch({
        type: GET_GRID_VBOM_VNF_INSTANCE_AND_CAPACITY,
        payload: {
          VBOMVnfInstanceAndCapacityGridResult: result,
          filter: null,
        } as VBOMVnfInstanceAndCapacityGrid,
      });
      setLoader("REMOVE", "GetVnfInfoAndCapacityDetails");
      return result;
    } else {
      setLoader("REMOVE", "GetVnfInfoAndCapacityDetails");
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
      type: GET_GRID_VBOM_VNF_INSTANCE_AND_CAPACITY,
      payload: {
        VBOMVnfInstanceAndCapacityGridResult: null,
        filter: null,
      } as VBOMVnfInstanceAndCapacityGrid,
    });
  }
  setLoader("REMOVE", "GetVnfInfoAndCapacityDetails");
}

export async function GetFilterColumnVBOMInfo(
  columName: string,
  columValue: string,
  queryFilter?: VBOMInfoQueryObjectGrid
) {
  let api = new VBOMInfoApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.VBOMInfoGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    VBOMInfoGridResult: null,
  } as VBOMInfoGrid;
  rootStore.dispatch({ type: GET_FILTER_VBOM_INFO, payload: rtn });

  return rtn;
}

export async function GetFilterColumnVBOMInstanceCapacity(
  columName: string,
  columValue: string,
  queryFilter?: VBOMInfoQueryObjectGrid,
  isInstance?: boolean
) {
  let api = new VBOMInfoApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.VBOMInfoGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue,
      (isInstance = true)
    )
  );
  let rtn = {
    filter: result,
    VBOMInfoGridResult: null,
  } as VBOMInfoGrid;
  rootStore.dispatch({
    type: GET_FILTER_VBOM_VNF_INSTANCE_AND_CAPACITY,
    payload: rtn,
  });

  return rtn;
}
export async function GetFilterColumnVBOMVnfCapacityFilter(
  columName: string,
  columValue: string,
  queryFilter?: VBOMInfoQueryObjectGrid,
  isInstance?: boolean
) {
  let api = new VBOMInfoApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.VBOMVnfCapacityFilter(
      queryFilter ?? {},
      columName,
      columValue,
      (isInstance = true)
    )
  );
  let rtn = {
    filter: result,
    VBOMInfoGridResult: null,
  } as VBOMInfoGrid;
  rootStore.dispatch({
    type: GET_FILTER_VBOM_VNF_CAPACITY,
    payload: rtn,
  });

  return rtn;
}

export async function GetFilterColumnVBOMClusterInfo(
  columName: string,
  columValue: string,
  queryFilter?: VBOMClusterInfoQueryObjectGrid
) {
  let api = new VBOMInfoApi();
  let result: FilterValueDto[] | undefined;
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.VBOMInfoGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    VBOMClusterInfoGridResult: null,
  } as VBOMClusterInfoGrid;
  rootStore.dispatch({ type: GET_FILTER_VBOM_CLUSTER_INFO, payload: rtn });

  return rtn;
}
