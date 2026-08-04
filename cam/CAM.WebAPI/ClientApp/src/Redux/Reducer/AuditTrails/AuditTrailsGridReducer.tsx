import {
  GET_GRID_AUDIT_TRAILS,
  AuditTrailsGrid,
  GET_FILTER_AUDIT_TRAILS,
} from "../../../Model/AuditTrails";

const initState: AuditTrailsGrid = {
  AuditTrailsGridResult: null,
  filter: null,
};

//const dispatch = useDispatch();

export const AuditTrailsGridReducer = (
  state = initState,
  action: { type: string; payload: AuditTrailsGrid }
) => {
  switch (action.type) {
    case GET_GRID_AUDIT_TRAILS: {
      return {
        ...state,
        AuditTrailsGridResult: action.payload.AuditTrailsGridResult,
      };
    }
    case GET_FILTER_AUDIT_TRAILS:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
