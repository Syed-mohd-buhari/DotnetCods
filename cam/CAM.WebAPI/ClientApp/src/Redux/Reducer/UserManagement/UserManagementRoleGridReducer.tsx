import {
  GET_GRID_USER_MANAGEMENT_ROLE,
  GET_FILTER_USER_MANAGEMENT_ROLE,
  UserManagementRoleGrid,
} from "../../../Model/UserManagement";

const initState: UserManagementRoleGrid = {
  UserManagementRoleGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const UserManagementRoleGridReducer = (
  state = initState,
  action: { type: string; payload: UserManagementRoleGrid }
) => {
  //console.log("Tems Reducer 2");
  switch (action.type) {
    case GET_GRID_USER_MANAGEMENT_ROLE: {
      return {
        ...state,
        UserManagementRoleGridResult: action.payload.UserManagementRoleGridResult,
      };
    }
    case GET_FILTER_USER_MANAGEMENT_ROLE:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
