import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { TeamsApi } from "../../../Business/TeamManagementBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  GET_CREATE_TEAMS,
  CREATE_TEAMS,
  TeamsDtoCreate,
  TeamsCreate,
} from "../../../Model/TeamManagement";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetTeamsCreateResource() {
  setLoader("ADD", "GetTeamsCreateResource");

  let api = new TeamsApi();
  let createResource = await ApiCallWithErrorHandling<Promise<TeamsDtoCreate>>(
    () => api.teamsGetCreateResource()
  );
  let rtn = {
    ResultDtoCreate: null,
    TeamsDtoCreate: createResource,
  } as TeamsCreate;
  rootStore.dispatch({ type: GET_CREATE_TEAMS, payload: rtn });
  setLoader("REMOVE", "GetTeamsCreateResource");

  return rtn.TeamsDtoCreate;
}

export async function CreateTeams(data: TeamsDtoCreate, forced?: boolean) {
  setLoader("ADD", "CreateTeams");
  let api = new TeamsApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.teamsCreateOrUpdateTeams(data, forced)
  );
  let rtn = {
    ResultDtoCreate: result,
    TeamsDtoCreate: null,
  } as TeamsCreate;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: CREATE_TEAMS, payload: rtn });
  setLoader("REMOVE", "CreateTeams");
  return rtn;
}
