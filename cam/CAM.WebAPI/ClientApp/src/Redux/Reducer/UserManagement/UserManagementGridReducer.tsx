import {
  GET_FILTER_USER_MANAGEMENT,
  GET_GRID_USER_MANAGEMENT,
  UserManagementGrid,
} from "../../../Model/UserManagement";

const initState: UserManagementGrid = {
  UserManagementGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const UserManagementGridReducer = (
  state = initState,
  action: { type: string; payload: UserManagementGrid }
) => {
  //console.log("Tems Reducer 2");
  switch (action.type) {
    case GET_GRID_USER_MANAGEMENT: {
      return {
        ...state,
        UserManagementGridResult: action.payload.UserManagementGridResult,
      };
    }
    case GET_FILTER_USER_MANAGEMENT:
      return { ...state, filter: action.payload.filter };

    default:
      return state;
  }
};
