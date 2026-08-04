import {
  EDIT_DESIGN_ASPECT,
  GET_EDIT_DESIGN_ASPECT,
  DesignAspectEdit,
} from "../../../Model/DesignAspects";

const initState: DesignAspectEdit = {
  DesignAspectDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const DesignAspectEditReducer = (
  state = initState,
  action: { type: string; payload: DesignAspectEdit }
) => {
  switch (action.type) {
    case EDIT_DESIGN_ASPECT: {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case GET_EDIT_DESIGN_ASPECT:
      return {
        ...state,
        DesignAspectDtoEdit: action.payload.DesignAspectDtoEdit,
      };
    default:
      return state;
  }
};
