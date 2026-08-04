import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { InfraClusterApi } from "../../../Business/InfraClusterBuisness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  CREATE_INFRA_CLUSTER,
  GET_CREATE_INFRA_CLUSTER,
  GET_CREATE_INFRA_CLUSTER_HARDWARE,
  GET_CREATE_PROGRAM_CLUSTER,
  GET_GRID_INFRA_CLUSTER,
  GET_GRID_PROGRAM_CLUSTER,
  GET_GRID_UPDATE_HARDWARE_CLUSTER,
  InfraClusterCreate,
  InfraClusterDtoUpdate,
  InfraClusterGrid,
  InfraClusterQueryDto,
  QueryResultDtoOfInfraClusterDtoGrid,
} from "../../../Model/InfraCluster";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function InfraClusterResource(data: any, forced?: boolean) {
  setLoader("ADD", "InfraClusterResource");
  let api = new InfraClusterApi();
  let createResource = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.InfraClusterCreateResource(data, forced)
  );
  let rtn = {
    ResultDtoCreate: null,
    InfraClusterDtoCreate: createResource,
  } as InfraClusterCreate;
  rootStore.dispatch({ type: GET_CREATE_INFRA_CLUSTER, payload: rtn });
  setLoader("REMOVE", "InfraClusterResource");
  return rtn;
}

export async function InfraClusterPaLevelGet(
  queryFilter?: InfraClusterDtoUpdate
) {
  let api = new InfraClusterApi();
  let result: QueryResultDtoOfInfraClusterDtoGrid | null | undefined;
  setLoader("ADD", "InfraClusterPaLevelGet");
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfInfraClusterDtoGrid>
    >(() => api.GetInfraClusterPaLevel(queryFilter ?? {}));
    let rtn = {
      InfraClusterGridResult: result,
      filter: null,
    } as InfraClusterGrid;
    rootStore.dispatch({ type: GET_GRID_INFRA_CLUSTER, payload: rtn });
    setLoader("REMOVE", "InfraClusterPaLevelGet");
    return rtn;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_INFRA_CLUSTER,
      payload: {
        InfraClusterGridResult: null,
        filter: null,
      } as InfraClusterGrid,
    });
  }
  setLoader("REMOVE", "InfraClusterPaLevelGet");
}

export async function InfraClusterHardwareResource(
  data: any,
  forced?: boolean
) {
  setLoader("ADD", "InfraClusterHardwareResource");
  let api = new InfraClusterApi();
  let createResource = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.InfraClusterHardwareCreateResource(data, forced)
  );
  let rtn = {
    ResultDtoCreate: null,
    InfraClusterDtoCreate: createResource,
  } as InfraClusterCreate;
  rootStore.dispatch({ type: GET_CREATE_INFRA_CLUSTER_HARDWARE, payload: rtn });
  setLoader("REMOVE", "InfraClusterHardwareResource");
  return rtn;
}

export async function InfraClusterPaHardwareLevelGet(
  queryFilter?: InfraClusterDtoUpdate
) {
  let api = new InfraClusterApi();
  let result: QueryResultDtoOfInfraClusterDtoGrid | null | undefined;
  setLoader("ADD", "InfraClusterPaHardwareLevelGet");
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfInfraClusterDtoGrid>
    >(() => api.GetInfraHardwareClusterPaLevel(queryFilter ?? {}));
    let rtn = {
      InfraClusterGridResult: result,
      filter: null,
    } as InfraClusterGrid;
    rootStore.dispatch({
      type: GET_GRID_UPDATE_HARDWARE_CLUSTER,
      payload: rtn,
    });
    setLoader("REMOVE", "InfraClusterPaHardwareLevelGet");
    return rtn;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_UPDATE_HARDWARE_CLUSTER,
      payload: {
        InfraClusterGridResult: null,
        filter: null,
      } as InfraClusterGrid,
    });
  }
  setLoader("REMOVE", "InfraClusterPaHardwareLevelGet");
}

export async function InfraClusterPaProgramLevelGet(
  queryFilter?: InfraClusterDtoUpdate
) {
  let api = new InfraClusterApi();
  let result: QueryResultDtoOfInfraClusterDtoGrid | null | undefined;
  setLoader("ADD", "InfraClusterPaProgramLevelGet");
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfInfraClusterDtoGrid>
    >(() => api.GetInfraProgramClusterPaLevel(queryFilter ?? {}));
    let rtn = {
      InfraClusterGridResult: result,
      filter: null,
    } as InfraClusterGrid;
    rootStore.dispatch({
      type: GET_GRID_UPDATE_HARDWARE_CLUSTER,
      payload: rtn,
    });
    setLoader("REMOVE", "InfraClusterPaProgramLevelGet");
    return rtn;
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_PROGRAM_CLUSTER,
      payload: {
        InfraClusterGridResult: null,
        filter: null,
      } as InfraClusterGrid,
    });
  }
  setLoader("REMOVE", "InfraClusterPaProgramLevelGet");
}

export async function InfraProgramClusterResource(data: any, forced?: boolean) {
  setLoader("ADD", "InfraProgramClusterResource");
  let api = new InfraClusterApi();
  let createResource = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.InfraProgramClusterCreateResource(data, forced)
  );
  let rtn = {
    ResultDtoCreate: null,
    InfraClusterDtoCreate: createResource,
  } as InfraClusterCreate;
  rootStore.dispatch({ type: GET_CREATE_PROGRAM_CLUSTER, payload: rtn });
  setLoader("REMOVE", "InfraProgramClusterResource");
  return rtn;
}
