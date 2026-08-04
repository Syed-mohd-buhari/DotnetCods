import {
  CREATE_ORGANIZATION_INFO,
  GET_CREATE_ORGANIZATION_INFO,
  OrganizationInfoCreate,
} from "../../../Model/OrganizationInfo";

const initState: OrganizationInfoCreate = {
  ResultDtoCreate: null,
  OrganizationInfoDtoCreate: null,
};
//const dispatch = useDispatch();

export const OrganizationInfoCreateReducer = (
  state = initState,
  action: { type: string; payload: OrganizationInfoCreate }
) => {
  switch (action.type) {
    case CREATE_ORGANIZATION_INFO: {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case GET_CREATE_ORGANIZATION_INFO:
      return {
        ...state,
        OrganizationInfoDtoCreate: action.payload.OrganizationInfoDtoCreate,
      };
    default:
      return state;
  }
};
