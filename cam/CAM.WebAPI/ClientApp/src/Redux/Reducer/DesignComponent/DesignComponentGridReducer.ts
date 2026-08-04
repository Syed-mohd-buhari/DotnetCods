import {
  GET_FILTER_DESIGN_COMPONENT,
  GET_GRID_DESIGN_COMPONENT,
  DesignComponentGrid,
} from "../../../Model/DesignComponent";

const initState: DesignComponentGrid = {
  DesignComponentGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const DesignComponentGridReducer = (
  state = initState,
  action: { type: string; payload: DesignComponentGrid }
) => {
  switch (action.type) {
    case GET_GRID_DESIGN_COMPONENT: {
      return {
        ...state,
        DesignComponentGridResult: action.payload.DesignComponentGridResult,
      };
    }
    case GET_FILTER_DESIGN_COMPONENT:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
