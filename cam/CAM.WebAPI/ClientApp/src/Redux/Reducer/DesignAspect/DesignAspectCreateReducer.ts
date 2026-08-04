import {
  CREATE_DESIGN_ASPECT,
  GET_CREATE_DESIGN_ASPECT,
  DesignAspectCreate,
} from "../../../Model/DesignAspects";

const initState: DesignAspectCreate = {
  ResultDtoCreate: null,
  DesignAspectDtoCreate: null,
};
//const dispatch = useDispatch();

export const DesignAspectCreateReducer = (
  state = initState,
  action: { type: string; payload: DesignAspectCreate }
) => {
  switch (action.type) {
    case CREATE_DESIGN_ASPECT: {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case GET_CREATE_DESIGN_ASPECT:
      return {
        ...state,
        DesignAspectDtoCreate: action.payload.DesignAspectDtoCreate,
      };
    default:
      return state;
  }
};
