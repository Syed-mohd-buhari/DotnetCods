import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { LcmEngineeringApi } from "../../../Business/LcmEngineeringBusiness";
import {
  GET_FILTER_LCM_ENGINEERING,
  GET_GRID_LCM_ENGINEERING,
  LcmEngineeringGrid,
  LcmEngineringQueryObjectGrid,
  QueryResultDtoOfLcmEngineeringDtoGrid,
} from "../../../Model/LcmEngineering";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetLcmEngineeringGrid(
  queryFilter?: LcmEngineringQueryObjectGrid
) {
  let api = new LcmEngineeringApi();
  let result: QueryResultDtoOfLcmEngineeringDtoGrid | null | undefined;
  setLoader("ADD", "GetLcmEngineeringGrid");
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfLcmEngineeringDtoGrid>
    >(() => api.lcmEngineeringGetLcmengineering(queryFilter ?? {}));
    let rtn = {
      LcmEngineeringGridResult: result,
      filter: null,
    } as LcmEngineeringGrid;
    rootStore.dispatch({ type: GET_GRID_LCM_ENGINEERING, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_LCM_ENGINEERING,
      payload: {
        LcmEngineeringGridResult: null,
        filter: null,
      } as LcmEngineeringGrid,
    });
  }
  setLoader("REMOVE", "GetLcmEngineeringGrid");
}

export async function GetFilterColumLcmEngineering(
  columName: string,
  columValue: string,
  queryFilter?: LcmEngineringQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new LcmEngineeringApi();

  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.lcmEngineeringGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    LcmEngineeringGridResult: null,
  } as LcmEngineeringGrid;
  rootStore.dispatch({ type: GET_FILTER_LCM_ENGINEERING, payload: rtn });
}

export async function GetFilterColumLcmEngineeringArchived(
  columName: string,
  columValue: string,
  queryFilter?: LcmEngineringQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new LcmEngineeringApi();

  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.archivedLcmGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    LcmEngineeringGridResult: null,
  } as LcmEngineeringGrid;
  rootStore.dispatch({ type: GET_FILTER_LCM_ENGINEERING, payload: rtn });
}

export async function GetArchivedLcmEngineeringGrid(
  queryFilter?: LcmEngineringQueryObjectGrid
) {
  let api = new LcmEngineeringApi();
  let result: QueryResultDtoOfLcmEngineeringDtoGrid | null | undefined;
  setLoader("ADD", "GetArchivedLcmEngineeringGrid");
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfLcmEngineeringDtoGrid>
    >(() => api.archivedGetLcmengineeringArchived(queryFilter ?? {}));
    let rtn = {
      LcmEngineeringGridResult: result,
      filter: null,
    } as LcmEngineeringGrid;
    rootStore.dispatch({ type: GET_GRID_LCM_ENGINEERING, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_LCM_ENGINEERING,
      payload: {
        LcmEngineeringGridResult: null,
        filter: null,
      } as LcmEngineeringGrid,
    });
  }
  setLoader("REMOVE", "GetArchivedLcmEngineeringGrid");
}
