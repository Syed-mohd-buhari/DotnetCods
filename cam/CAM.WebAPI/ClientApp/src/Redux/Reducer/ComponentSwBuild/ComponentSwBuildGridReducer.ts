import {
  GET_FILTER_COMPONENT_SW_BUILD,
  GET_GRID_COMPONENT_SW_BUILD,
  ComponentSwBuildGrid,
} from "../../../Model/ComponentSwBuild";

const initState: ComponentSwBuildGrid = {
  ComponentSwBuildGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const ComponentSwBuildGridReducer = (
  state = initState,
  action: { type: string; payload: ComponentSwBuildGrid }
) => {
  switch (action.type) {
    case GET_GRID_COMPONENT_SW_BUILD: {
      return {
        ...state,
        ComponentSwBuildGridResult: action.payload.ComponentSwBuildGridResult,
      };
    }
    case GET_FILTER_COMPONENT_SW_BUILD:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
