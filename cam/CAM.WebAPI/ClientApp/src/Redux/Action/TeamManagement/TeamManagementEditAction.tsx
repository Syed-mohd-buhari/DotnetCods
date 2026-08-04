import { ApiCallWithErrorHandling } from "../../../Business/Common/CommonBusiness";
import { TeamsApi } from "../../../Business/TeamManagementBusiness";
import { ResultDto } from "../../../Model/CommonModels";
import {
  EDIT_TEAMS,
  GET_EDIT_TEAMS,
  TeamsDtoEdit,
  TeamsEdit,
} from "../../../Model/TeamManagement";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetTeamsEditResource(id: number) {
  setLoader("ADD", "GetTeamsEditResource");

  let api = new TeamsApi();
  let editResource = await ApiCallWithErrorHandling<Promise<TeamsDtoEdit>>(() =>
    api.teamsGetUpdateResource(id)
  );
  let rtn = { TeamsDtoEdit: editResource } as TeamsEdit;
  rootStore.dispatch({ type: GET_EDIT_TEAMS, payload: rtn });
  setLoader("REMOVE", "GetTeamsEditResource");

  return rtn;
}

export async function EditTeams(data: TeamsDtoEdit, forced?: boolean) {
  setLoader("ADD", "EditTeams");
  let api = new TeamsApi();
  let result = await ApiCallWithErrorHandling<Promise<ResultDto>>(() =>
    api.teamsCreateOrUpdateTeams(data, forced)
  );
  let rtn = { ResultDtoEdit: result } as TeamsEdit;
  rootStore.dispatch(
    setNotification({
      message: result?.info ?? "",
      notifyType: result?.warning ? NotifyType.error : NotifyType.success,
    })
  );
  rootStore.dispatch({ type: EDIT_TEAMS, payload: rtn });
  setLoader("REMOVE", "EditTeams");
  return rtn;
}
