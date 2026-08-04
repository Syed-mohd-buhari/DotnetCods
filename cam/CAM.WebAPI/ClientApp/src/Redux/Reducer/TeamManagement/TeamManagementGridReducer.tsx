import {
  GET_FILTER_TEAMS,
  GET_GRID_TEAMS,
  TeamsGrid,
} from "../../../Model/TeamManagement";

const initState: TeamsGrid = {
  TeamsGridResult: null,
  filter: null,
};

export const TeamManagementGridReducer = (
  state = initState,
  action: { type: string; payload: TeamsGrid }
) => {
  switch (action.type) {
    case GET_GRID_TEAMS: {
      return {
        ...state,
        TeamsGridResult: action.payload.TeamsGridResult,
      };
    }
    case GET_FILTER_TEAMS:
      return { ...state, filter: action.payload.filter };

    default:
      return state;
  }
};
