import { type } from "os";
import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import {
  SettingsUpdatePlannedActivityApiFetchParamCreator,
  SettingsUpdatePlannedActivityApi,
} from "../../../Business/SettingsUpdatePlannedActivityBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  CREATE_SETTINGS_UPDATE_PLANNED_ACTIVITY,
  SettingsUpdatePlannedActivityCreate,
  SettingsUpdatePlannedActivityDtoCreate,
  GET_CREATE_SETTINGS_UPDATE_PLANNED_ACTIVITY,
} from "../../../Model/SettingsUpdatePlannedActivity";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function GetSettingsUpdatePlannedActivityCreateResource({
  isRefillData,
}: {
  isRefillData?: boolean;
} = {}) {
  setLoader("ADD", "GetSettingsUpdatePlannedActivityCreateResource");

  let api = new SettingsUpdatePlannedActivityApi();
  let createResource = await ApiCallWithErrorHandling<
    Promise<SettingsUpdatePlannedActivityDtoCreate>
  >(() =>
    api.settingsUpdatePlannedActivityGetCreateResourceSettingsUpdatePlannedActivity()
  );
  let rtn = {
    ResultDtoCreate: null,
    SettingsUpdatePlannedActivityDtoCreate: createResource,
  } as SettingsUpdatePlannedActivityCreate;
  if (!isRefillData || isRefillData === undefined) {
    rootStore.dispatch({
      type: GET_CREATE_SETTINGS_UPDATE_PLANNED_ACTIVITY,
      payload: rtn,
    });
  }
  setLoader("REMOVE", "GetSettingsUpdatePlannedActivityCreateResource");

  return rtn.SettingsUpdatePlannedActivityDtoCreate;
}

export async function CreatSettingsUpdatePlannedActivity(
  data: SettingsUpdatePlannedActivityDtoCreate,
  forced?: boolean
) {
  setLoader("ADD", "CreatSettingsUpdatePlannedActivity");
  let api = new SettingsUpdatePlannedActivityApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.settingsUpdatePlannedActivityCreate(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    SettingsUpdatePlannedActivityDtoCreate: null,
  } as SettingsUpdatePlannedActivityCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({
    type: CREATE_SETTINGS_UPDATE_PLANNED_ACTIVITY,
    payload: rtn,
  });
  setLoader("REMOVE", "CreatSettingsUpdatePlannedActivity");
  return rtn;
}
