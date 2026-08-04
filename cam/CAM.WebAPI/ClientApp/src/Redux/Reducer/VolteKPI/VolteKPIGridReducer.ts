import {
  GET_FILTER_VOLTE_KPI,
  GET_GRID_VOLTE_KPI,
  VolteKPIGrid,
} from "../../../Model/VolteKpi/VolteKPI";

const initState: VolteKPIGrid = {
  VolteKPIGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const VolteKPIGridReducer = (
  state = initState,
  action: { type: string; payload: VolteKPIGrid }
) => {
  switch (action.type) {
    case GET_GRID_VOLTE_KPI: {
      return {
        ...state,
        VolteKPIGridResult: action.payload.VolteKPIGridResult,
      };
    }
    case GET_FILTER_VOLTE_KPI:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
