import React from "react";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import { SettingsUpdatePlannedActivityApi } from "../../../Business/SettingsUpdatePlannedActivityBusiness";
import {
  SettingsUpdatePlannedActivityGrid,
  SettingsUpdatePlannedActivityQueryObjectGrid,
  GET_FILTER_SETTINGS_UPDATE_PLANNED_ACTIVITY,
  GET_GRID_SETTINGS_UPDATE_PLANNED_ACTIVITY,
  QueryResultDtoOfSettingsUpdatePlannedActivityDtoGrid,
} from "../../../Model/SettingsUpdatePlannedActivity";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSettingsUpdatePlannedActivityGrid(
  queryFilter?: SettingsUpdatePlannedActivityQueryObjectGrid
) {
  setLoader("ADD", "GetSettingsUpdatePlannedActivityGrid");

  let result:
    | QueryResultDtoOfSettingsUpdatePlannedActivityDtoGrid
    | null
    | undefined;
  let api = new SettingsUpdatePlannedActivityApi();

  try {
    if (queryFilter !== null && queryFilter !== undefined) {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfSettingsUpdatePlannedActivityDtoGrid>
      >(() =>
        api.settingsUpdatePlannedActivityGetSettingsUpdatePlannedActivity(
          queryFilter.ruleElementCount,
          queryFilter.plannedActivityTypeFor,
          queryFilter.lcmDeploymentStatus,
          queryFilter.planningActivityResource,
          queryFilter?.plannedActivityTypeDescription,
          queryFilter?.successorPlannedActivityTypeResource,
          queryFilter?.ruleforSuccessorPlannedActivityCreation,
          queryFilter.rule,
          queryFilter.maxOrder,
          queryFilter.order,
          queryFilter.deliveryStatus,
          queryFilter.msStatus,
          queryFilter.msStatusDuration,
          queryFilter?.settingsUpdatePlannedActivityDescription,
          queryFilter?.planningActivityStatus,
          queryFilter?.budgetAvailability,
          queryFilter?.localApproval,
          queryFilter?.deliveryStatusId,
          queryFilter?.lastModifiedBy,
          queryFilter?.crossSetting,
          queryFilter?.sortBy,
          queryFilter?.isSortAscending,
          queryFilter?.page,
          queryFilter?.pageSize,
          queryFilter?.lastModifiedStartDate,
          queryFilter?.lastModifiedEndDate,
          queryFilter?.principalId,
          queryFilter?.deleted,
          queryFilter?.orphan
        )
      );
    } else {
      result = await ApiCallWithErrorHandling<
        Promise<QueryResultDtoOfSettingsUpdatePlannedActivityDtoGrid>
      >(() =>
        api.settingsUpdatePlannedActivityGetSettingsUpdatePlannedActivity()
      );
    }

    let rtn = {
      SettingsUpdatePlannedActivityGridResult: result,
      filter: null,
    } as SettingsUpdatePlannedActivityGrid;
    rootStore.dispatch({
      type: GET_GRID_SETTINGS_UPDATE_PLANNED_ACTIVITY,
      payload: rtn as SettingsUpdatePlannedActivityGrid,
    });
  } catch (error) {
    rootStore.dispatch({
      type: GET_GRID_SETTINGS_UPDATE_PLANNED_ACTIVITY,
      payload: {
        SettingsUpdatePlannedActivityGridResult: result,
        filter: null,
      } as SettingsUpdatePlannedActivityGrid,
    });
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
  }
  setLoader("REMOVE", "GetSettingsUpdatePlannedActivityGrid");
}

export async function GetFilterColumSettingsUpdatePlannedActivity(
  columName: string,
  columValue: string,
  queryFilter?: SettingsUpdatePlannedActivityQueryObjectGrid
) {
  let result: FilterValueDto[] | undefined;
  let api = new SettingsUpdatePlannedActivityApi();

  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.settingsUpdatePlannedActivityGetFilterResult(
      queryFilter ?? {},
      columName,
      columValue
    )
  );

  let rtn = {
    filter: result,
    SettingsUpdatePlannedActivityGridResult: null,
  } as SettingsUpdatePlannedActivityGrid;
  rootStore.dispatch({
    type: GET_FILTER_SETTINGS_UPDATE_PLANNED_ACTIVITY,
    payload: rtn,
  });
}
