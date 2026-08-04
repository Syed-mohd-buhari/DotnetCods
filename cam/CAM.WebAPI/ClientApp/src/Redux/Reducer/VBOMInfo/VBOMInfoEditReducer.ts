import {
  EDIT_VBOM_INFO,
  GET_EDIT_VBOM_INFO,
  VBOMInfoEdit,
} from "../../../Model/VBOMInfo";

const initState: VBOMInfoEdit = {
  VBOMInfoDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const VBOMInfoEditReducer = (
  state = initState,
  action: { type: string; payload: VBOMInfoEdit }
) => {
  switch (action.type) {
    case EDIT_VBOM_INFO: {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case GET_EDIT_VBOM_INFO:
      return {
        ...state,
        VBOMInfoDtoEdit: action.payload.VBOMInfoDtoEdit,
      };
    default:
      return state;
  }
};
