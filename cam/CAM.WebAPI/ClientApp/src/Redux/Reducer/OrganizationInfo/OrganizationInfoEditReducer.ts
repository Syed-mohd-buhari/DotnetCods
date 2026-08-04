import {
  EDIT_ORGANIZATION_INFO,
  GET_EDIT_ORGANIZATION_INFO,
  OrganizationInfoEdit,
} from "../../../Model/OrganizationInfo";

const initState: OrganizationInfoEdit = {
  OrganizationInfoDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const OrganizationInfoEditReducer = (
  state = initState,
  action: { type: string; payload: OrganizationInfoEdit }
) => {
  switch (action.type) {
    case EDIT_ORGANIZATION_INFO: {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case GET_EDIT_ORGANIZATION_INFO:
      return {
        ...state,
        OrganizationInfoDtoEdit: action.payload.OrganizationInfoDtoEdit,
      };
    default:
      return state;
  }
};
