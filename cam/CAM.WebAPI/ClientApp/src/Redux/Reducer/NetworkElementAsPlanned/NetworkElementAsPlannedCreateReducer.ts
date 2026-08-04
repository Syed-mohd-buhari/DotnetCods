import {
  CREATE_NETWORK_ELEMENT_AS_PLANNED,
  GET_CREATE_NETWORK_ELEMENT_AS_PLANNED,
  NetworkElementAsPlannedCreate,
} from "../../../Model/NetworkElementAsPlanned";

const initState: NetworkElementAsPlannedCreate = {
  ResultDtoCreate: null,
  NetworkElementAsPlannedDtoCreate: null,
};
//const dispatch = useDispatch();

export const NetworkElementAsPlannedCreateReducer = (
  state = initState,
  action: { type: string; payload: NetworkElementAsPlannedCreate }
) => {
  switch (action.type) {
    case CREATE_NETWORK_ELEMENT_AS_PLANNED: {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case GET_CREATE_NETWORK_ELEMENT_AS_PLANNED:
      return {
        ...state,
        NetworkElementAsPlannedDtoCreate:
          action.payload.NetworkElementAsPlannedDtoCreate,
      };
    default:
      return state;
  }
};
