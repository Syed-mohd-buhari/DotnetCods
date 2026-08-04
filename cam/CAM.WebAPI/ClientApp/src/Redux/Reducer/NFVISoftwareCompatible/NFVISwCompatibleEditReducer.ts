import {
  EDIT_NFVI_SW_COMPATIBLE,
  GET_EDIT_NFVI_SW_COMPATIBLE,
  NFVISwCompatibleEdit,
} from "../../../Model/NFVISoftwareCompatible";

const initState: NFVISwCompatibleEdit = {
  NFVISwCompatibleDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const NFVISwCompatibleEditReducer = (
  state = initState,
  action: { type: string; payload: NFVISwCompatibleEdit }
) => {
  switch (action.type) {
    case EDIT_NFVI_SW_COMPATIBLE: {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case GET_EDIT_NFVI_SW_COMPATIBLE:
      return {
        ...state,
        NFVISwCompatibleDtoEdit: action.payload.NFVISwCompatibleDtoEdit,
      };
    default:
      return state;
  }
};
