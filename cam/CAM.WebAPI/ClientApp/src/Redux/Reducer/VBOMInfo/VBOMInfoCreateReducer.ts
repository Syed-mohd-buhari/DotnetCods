import {
  CREATE_VBOM_INFO,
  GET_CREATE_VBOM_INFO,
  VBOMInfoCreate,
} from "../../../Model/VBOMInfo";

const initState: VBOMInfoCreate = {
  ResultDtoCreate: null,
  VBOMInfoDtoCreate: null,
};
//const dispatch = useDispatch();

export const VBOMInfoCreateReducer = (
  state = initState,
  action: { type: string; payload: VBOMInfoCreate }
) => {
  switch (action.type) {
    case CREATE_VBOM_INFO: {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case GET_CREATE_VBOM_INFO:
      return {
        ...state,
        VBOMInfoDtoCreate: action.payload.VBOMInfoDtoCreate,
      };
    default:
      return state;
  }
};
