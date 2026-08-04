import {
  EDIT_COMPONENT_SW_BUILD,
  GET_EDIT_COMPONENT_SW_BUILD,
  ComponentSwBuildEdit,
} from "../../../Model/ComponentSwBuild";

const initState: ComponentSwBuildEdit = {
  ComponentSwBuildDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const ComponentSwBuildEditReducer = (
  state = initState,
  action: { type: string; payload: ComponentSwBuildEdit }
) => {
  switch (action.type) {
    case EDIT_COMPONENT_SW_BUILD: {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case GET_EDIT_COMPONENT_SW_BUILD:
      return {
        ...state,
        ComponentSwBuildDtoEdit: action.payload.ComponentSwBuildDtoEdit,
      };
    default:
      return state;
  }
};
