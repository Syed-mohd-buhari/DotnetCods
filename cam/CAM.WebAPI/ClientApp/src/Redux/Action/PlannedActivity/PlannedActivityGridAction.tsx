import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { PlannedActivityApi } from "../../../Business/PlannedActivityBusiness";
import {
  GET_FILTER_PLANNED_ACTIVITY,
  GET_GRID_PLANNED_ACTIVITY,
  PlannedActivityGrid,
  PlannedActivityQueryObjectGrid,
  QueryResultDtoOfPlannedActivityDtoGrid,
} from "../../../Model/PlannedActivity";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetPlannedActivityGrid(
  queryFilter?: PlannedActivityQueryObjectGrid
) {
  setLoader("ADD", "GetPlannedActivityGrid");

  let result: QueryResultDtoOfPlannedActivityDtoGrid | null | undefined;
  let api = new PlannedActivityApi();
  // console.log(JSON.stringify(queryFilter));
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfPlannedActivityDtoGrid>
    >(() => api.plannedActivityGetPlannedActivity(queryFilter ?? {}));

    let rtn = {
      PlannedActivityGridResult: result,
      filter: null,
    } as PlannedActivityGrid;
    rootStore.dispatch({ type: GET_GRID_PLANNED_ACTIVITY, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_PLANNED_ACTIVITY,
      payload: {
        PlannedActivityGridResult: null,
        filter: null,
      } as PlannedActivityGrid,
    });
  }
  setLoader("REMOVE", "GetPlannedActivityGrid");
}

export async function GetPlannedActivityGridForChart(
  queryFilter?: PlannedActivityQueryObjectGrid
) {
  setLoader("ADD", "GetPlannedActivityGrid");

  let result: QueryResultDtoOfPlannedActivityDtoGrid | null | undefined;
  let api = new PlannedActivityApi();
  // console.log(JSON.stringify(queryFilter));
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfPlannedActivityDtoGrid>
    >(() => api.plannedActivityGetPlannedActivity(queryFilter ?? {}));

    let rtn = {
      PlannedActivityGridResult: result,
      filter: null,
    } as PlannedActivityGrid;
    setLoader("REMOVE", "GetPlannedActivityGrid");

    return rtn;
    // rootStore.dispatch({ type: GET_GRID_PLANNED_ACTIVITY, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_PLANNED_ACTIVITY,
      payload: {
        PlannedActivityGridResult: null,
        filter: null,
      } as PlannedActivityGrid,
    });
  }
  setLoader("REMOVE", "GetPlannedActivityGrid");
}

export async function GetPlannedActivityArchivedGrid(
  queryFilter?: PlannedActivityQueryObjectGrid
) {
  setLoader("ADD", "GetPlannedActivityGrid");

  let result: QueryResultDtoOfPlannedActivityDtoGrid | null | undefined;
  let api = new PlannedActivityApi();
  // console.log(JSON.stringify(queryFilter));
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfPlannedActivityDtoGrid>
    >(() => api.plannedActivityGetArchivedPlannedActivity(queryFilter ?? {}));

    let rtn = {
      PlannedActivityGridResult: result,
      filter: null,
    } as PlannedActivityGrid;
    rootStore.dispatch({ type: GET_GRID_PLANNED_ACTIVITY, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_PLANNED_ACTIVITY,
      payload: {
        PlannedActivityGridResult: null,
        filter: null,
      } as PlannedActivityGrid,
    });
  }
  setLoader("REMOVE", "GetPlannedActivityGrid");
}

export async function GetFilterColumPlannedActivity(
  columName: string,
  columValue: string,
  queryFilter?: PlannedActivityQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumPlannedActivity");

  let result: FilterValueDto[] | undefined;
  let api = new PlannedActivityApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.plannedActivityGetFilterResult(
      {
        ...queryFilter,
        archived:
          sessionStorage.getItem("isArchivedMode") &&
          sessionStorage.getItem("archivedType") === "PA"
            ? true
            : false,
      } ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    PlannedActivityGridResult: null,
  } as PlannedActivityGrid;
  rootStore.dispatch({ type: GET_FILTER_PLANNED_ACTIVITY, payload: rtn });
  // setLoader("REMOVE", "GetFilterColumPlannedActivity");
}

export async function GetFilterColumPlannedActivityForChart(
  columName: string,
  columValue: string,
  queryFilter?: PlannedActivityQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumPlannedActivity");

  let result: FilterValueDto[] | undefined;
  let api = new PlannedActivityApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.plannedActivityGetFilterResult(
      {
        ...queryFilter,
        archived:
          sessionStorage.getItem("isArchivedMode") &&
          sessionStorage.getItem("archivedType") === "PA"
            ? true
            : false,
      } ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    PlannedActivityGridResult: null,
  } as PlannedActivityGrid;

  return rtn;
  // rootStore.dispatch({ type: GET_FILTER_PLANNED_ACTIVITY, payload: rtn });
  // setLoader("REMOVE", "GetFilterColumPlannedActivity");
}

export async function GetFilterColumArchivedPlannedActivity(
  columName: string,
  columValue: string,
  queryFilter?: PlannedActivityQueryObjectGrid
) {
  // setLoader("ADD", "GetFilterColumPlannedActivity");

  let result: FilterValueDto[] | undefined;
  let api = new PlannedActivityApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.plannedActivityGetArchivedFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );
  let rtn = {
    filter: result,
    PlannedActivityGridResult: null,
  } as PlannedActivityGrid;
  rootStore.dispatch({ type: GET_FILTER_PLANNED_ACTIVITY, payload: rtn });
  // setLoader("REMOVE", "GetFilterColumPlannedActivity");
}
