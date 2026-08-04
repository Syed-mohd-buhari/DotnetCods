import React from "react";
import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { TeamsApi } from "../../../Business/TeamManagementBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";
// import { useDispatch } from 'react-redux'

export async function DeleteTeamManagement(id: number) {
  setLoader("ADD", "DeleteTeamManagement");
  let api = new TeamsApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.teamsDelete(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "DELETE_TEAMMANAGEMENT", payload: rtn });
  setLoader("REMOVE", "DeleteTeamManagement");
  return rtn;
}

export async function DeleteDeepTeamManagement(id: number) {
  let api = new TeamsApi();
  setLoader("ADD", "DeleteDeepTeamManagement");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.teamsDeepDelete(id)
  );
  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: "DELETE_TEAMMANAGEMENT", payload: rtn });
  setLoader("REMOVE", "DeleteDeepTeamManagement");
  return rtn;
}

export async function GetRelatedRecordsTeamManagement(id: number) {
  console.log("GetRelatedRecordsTeamManagement called, id =", id);

  let api = new TeamsApi();
  setLoader("ADD", "GetRelatedRecordsTeamManagement");
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.teamsGetRelatedRecords(id)
  );
  console.log("GetRelatedRecordsTeamManagement result:", result);

  let rtn = {
    data: result?.data,
    info: result?.info,
    warning: result?.warning,
  } as ResultDto;
  if (result?.warning)
    rootStore.dispatch(
      setNotification({
        message: result?.info ?? "",
        notifyType: NotifyType.error,
      })
    );
  setLoader("REMOVE", "GetRelatedRecordsTeamManagement");
  return rtn;
}
