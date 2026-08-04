import {
  CREATE_COMPONENT_SW_BUILD,
  GET_CREATE_COMPONENT_SW_BUILD,
  ComponentSwBuildCreate,
} from "../../../Model/ComponentSwBuild";

const initState: ComponentSwBuildCreate = {
  ResultDtoCreate: null,
  ComponentSwBuildDtoCreate: null,
};
//const dispatch = useDispatch();

export const ComponentSwBuildCreateReducer = (
  state = initState,
  action: { type: string; payload: ComponentSwBuildCreate }
) => {
  switch (action.type) {
    case CREATE_COMPONENT_SW_BUILD: {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case GET_CREATE_COMPONENT_SW_BUILD:
      return {
        ...state,
        ComponentSwBuildDtoCreate: action.payload.ComponentSwBuildDtoCreate,
      };
    default:
      return state;
  }
};
