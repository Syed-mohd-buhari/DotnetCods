import { LookUpCreateUserRole } from "../../../Model/LookUp/LookUpGenericModel";
import {
  GET_GRID_USER_MANAGEMENT_ROLE,
  GET_FILTER_USER_MANAGEMENT_ROLE,
  UserManagementRoleGrid,
  CREATE_USER_MANAGEMENT_ROLE,
  GET_CREATE_USER_MANAGEMENT_ROLE,
  ADD_NEW_USER_MANAGEMENT
} from "../../../Model/UserManagement";

//const dispatch = useDispatch();
const initState: LookUpCreateUserRole = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
}
//const dispatch = useDispatch();

export const UserManagementRoleCreateReducer = (state = initState, action: { type: string, payload: LookUpCreateUserRole }) => {
    switch (action.type) {
        case CREATE_USER_MANAGEMENT_ROLE:
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case GET_CREATE_USER_MANAGEMENT_ROLE:
            return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate }
        case ADD_NEW_USER_MANAGEMENT:
            return {...state, ResultDtoCreate: action.payload.ResultDtoCreate}
        default:
            return state;
    }
}
