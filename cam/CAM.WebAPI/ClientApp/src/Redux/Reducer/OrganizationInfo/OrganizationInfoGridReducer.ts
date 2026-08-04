import {
  GET_FILTER_ORGANIZATION_INFO,
  GET_GRID_ORGANIZATION_INFO,
  OrganizationInfoGrid,
} from "../../../Model/OrganizationInfo";

const initialState: OrganizationInfoGrid = {
  OrganizationInfoGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const OrganizationInfoGridReducer = (
  state = initialState,
  action: { type: string; payload: OrganizationInfoGrid }
) => {
  switch (action.type) {
    case GET_GRID_ORGANIZATION_INFO: {
      return {
        ...state,
        OrganizationInfoGridResult: action.payload.OrganizationInfoGridResult,
      };
    }
    case GET_FILTER_ORGANIZATION_INFO:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
