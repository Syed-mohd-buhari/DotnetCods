import {
  EDIT_TEAMS,
  GET_EDIT_TEAMS,
  TeamsEdit,
} from "../../../Model/TeamManagement";

const initState: TeamsEdit = {
  TeamsDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const TeamManagementEditReducer = (
  state = initState,
  action: { type: string; payload: TeamsEdit }
) => {
  switch (action.type) {
    case EDIT_TEAMS: {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case GET_EDIT_TEAMS:
      return {
        ...state,
        TeamsDtoEdit: action.payload.TeamsDtoEdit,
      };
    default:
      return state;
  }
};
