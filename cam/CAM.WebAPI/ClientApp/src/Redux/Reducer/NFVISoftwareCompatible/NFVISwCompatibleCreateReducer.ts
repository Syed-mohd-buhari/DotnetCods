import {
  CREATE_NFVI_SW_COMPATIBLE,
  GET_CREATE_NFVI_SW_COMPATIBLE,
  NFVISwCompatibleCreate,
} from "../../../Model/NFVISoftwareCompatible";

const initState: NFVISwCompatibleCreate = {
  ResultDtoCreate: null,
  NFVISwCompatibleDtoCreate: null,
};
//const dispatch = useDispatch();

export const NFVISwCompatibleCreateReducer = (
  state = initState,
  action: { type: string; payload: NFVISwCompatibleCreate }
) => {
  switch (action.type) {
    case CREATE_NFVI_SW_COMPATIBLE: {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case GET_CREATE_NFVI_SW_COMPATIBLE:
      return {
        ...state,
        NFVISwCompatibleDtoCreate: action.payload.NFVISwCompatibleDtoCreate,
      };
    default:
      return state;
  }
};
