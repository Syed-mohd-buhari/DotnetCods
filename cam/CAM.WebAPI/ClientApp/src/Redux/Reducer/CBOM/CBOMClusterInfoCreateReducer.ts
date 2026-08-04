import { CREATE_CBOM, GET_CREATE_CBOM, CBOMCreate } from "../../../Model/CBOM";

const initState: CBOMCreate = {
  ResultDtoCreate: null,
  CBOMDtoCreate: null,
};
//const dispatch = useDispatch();

export const CBOMClusterInfoCreateReducer = (
  state = initState,
  action: { type: string; payload: CBOMCreate }
) => {
  switch (action.type) {
    case CREATE_CBOM: {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case GET_CREATE_CBOM:
      return {
        ...state,
        CBOMDtoCreate: action.payload.CBOMDtoCreate,
      };
    default:
      return state;
  }
};
