import {
  CREATE_TEAMS,
  GET_CREATE_TEAMS,
  TeamsCreate,
} from "../../../Model/TeamManagement";

const initState: TeamsCreate = {
  ResultDtoCreate: null,
  TeamsDtoCreate: null,
};
//const dispatch = useDispatch();

export const TeamManagementCreateReducer = (
  state = initState,
  action: { type: string; payload: TeamsCreate }
) => {
  switch (action.type) {
    case CREATE_TEAMS: {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case GET_CREATE_TEAMS:
      return {
        ...state,
        TeamsDtoCreate: action.payload.TeamsDtoCreate,
      };
    default:
      return state;
  }
};
