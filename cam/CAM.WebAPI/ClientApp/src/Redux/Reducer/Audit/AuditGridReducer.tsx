import {
  GET_GRID_AUDIT,
  GET_FILTER_NETWORK_ELEMENT_AS_IS,
  AuditGrid,
} from "../../../Model/Audit";

const initState: AuditGrid = {
  AuditGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

//Added newly for New Network Element

export const AuditGridReducer = (
  state = initState,
  action: { type: string; payload: AuditGrid }
) => {
  //console.log("Tems Reducer 2");
  switch (action.type) {
    case GET_GRID_AUDIT: {
      return { ...state, AuditGridResult: action.payload.AuditGridResult };
    }
    case GET_FILTER_NETWORK_ELEMENT_AS_IS:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
