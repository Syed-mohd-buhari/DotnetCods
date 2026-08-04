import { TeamsApi } from "../../../Business/TeamManagementBusiness";
import {
  ApiCallWithErrorHandling,
  FilterValueDto,
} from "../../../Business/Common/CommonBusiness";
import {
  GET_FILTER_TEAMS,
  GET_GRID_TEAMS,
  QueryDtoforTeam,
  QueryResultDtoOfTeamsGridDto,
  TeamsGrid,
} from "../../../Model/TeamManagement";
import { NotifyType } from "../../Reducer/NotificationReducer";
import { rootStore } from "../../Store/rootStore";
import setLoader from "../LoaderAction";
import { setNotification } from "../NotificationAction";

export async function GetTeamsGrid(queryFilter?: QueryDtoforTeam) {
  setLoader("ADD", "GetTeamsGrid");
  let api = new TeamsApi();

  let result: QueryResultDtoOfTeamsGridDto | null | undefined;
  try {
    result = await ApiCallWithErrorHandling<
      Promise<QueryResultDtoOfTeamsGridDto>
    >(() => api.teamsGet(queryFilter ?? {}));

    let rtn = {
      TeamsGridResult: result,
      filter: null,
    } as TeamsGrid;

    rootStore.dispatch({ type: GET_GRID_TEAMS, payload: rtn });
  } catch (error) {
    rootStore.dispatch(
      setNotification({
        message: "Fail to fetch",
        notifyType: NotifyType.error,
      })
    );
    rootStore.dispatch({
      type: GET_GRID_TEAMS,
      payload: {
        TeamsGridResult: null,
        filter: null,
      } as TeamsGrid,
    });
  }
  setLoader("REMOVE", "GetTeamsGrid");
}

export async function GetFilterColumTeams(
  columName: string,
  columValue: string,
  queryFilter?: QueryDtoforTeam
) {
  let result: FilterValueDto[] | undefined;
  let api = new TeamsApi();
  result = await ApiCallWithErrorHandling<Promise<FilterValueDto[]>>(() =>
    api.teamsGetFilterResult(queryFilter ?? {}, columName, columValue)
  );
  let rtn = {
    filter: result,
    TeamsGridResult: null,
  } as TeamsGrid;
  rootStore.dispatch({ type: GET_FILTER_TEAMS, payload: rtn });
}
