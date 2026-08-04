import {
  GET_GRID_DESIGN_ASPECT,
  GET_FILTER_DESIGN_ASPECT,
  DesignAspectGrid,
} from "../../../Model/DesignAspects";

const initState: DesignAspectGrid = {
  DesignAspectGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const DesignAspectGridReducer = (
  state = initState,
  action: { type: string; payload: DesignAspectGrid }
) => {
  switch (action.type) {
    case GET_GRID_DESIGN_ASPECT: {
      return {
        ...state,
        DesignAspectGridResult: action.payload.DesignAspectGridResult,
      };
    }
    case GET_FILTER_DESIGN_ASPECT:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
