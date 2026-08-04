import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import {
  PlannedActivityTypesGrid,
  PlannedActivityTypesQueryObjectGrid,
  GET_FILTER_PLANNED_ACTIVITY_TYPES,
  GET_GRID_PLANNED_ACTIVITY_TYPES,
  QueryResultDtoOfPlannedActivityTypesDtoGrid,
} from "../../../Model/PlannedActivityTypes";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
import { PlannedActivityTypesApi } from "../../../Business/PlannedActivityTypesBusiness";
// import { useDispatch } from 'react-redux'

export async function GetPlannedActivityTypesGrid(
  queryFilter?: PlannedActivityTypesQueryObjectGrid
) {
  setLoader("ADD", "GetPlannedActivityTypesGrid");

  let result:
    | QueryResultDtoOfPlannedActivityTypesDtoGrid
    | null
    | undefined;
  let api = new PlannedActivityTypesApi();

  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfPlannedActivityTypesDtoGrid>
      >(() =>
        api.PlannedActivityTypesGetGrid(queryFilter)
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfPlannedActivityTypesDtoGrid>
      >(() =>
        api.PlannedActivityTypesGetGrid(queryFilter)
      );
    }

    let rtn = {
      PlannedActivityTypesGridResult: result,
      filter: null,
    } as PlannedActivityTypesGrid;
    rootStore.dispatch({
      type: GET_GRID_PLANNED_ACTIVITY_TYPES,
      payload: rtn as PlannedActivityTypesGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: GET_GRID_PLANNED_ACTIVITY_TYPES,
      payload: {
        PlannedActivityTypesGridResult: result,
        filter: null,
      } as PlannedActivityTypesGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetPlannedActivityTypesGrid");
}

export async function GetFilterColumPlannedActivityTypes(
  columName: string,
  columValue: string,
  queryFilter?: PlannedActivityTypesQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new PlannedActivityTypesApi();

  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.PlannedActivityTypesGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );

  let rtn = {
    filter: result,
    PlannedActivityTypesGridResult: null,
  } as PlannedActivityTypesGrid;
  rootStore.dispatch({
    type: GET_FILTER_PLANNED_ACTIVITY_TYPES,
    payload: rtn,
  });
}
