import { EDIT_CBOM, GET_EDIT_CBOM, CBOMEdit } from "../../../Model/CBOM";

const initState: CBOMEdit = {
  CBOMDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const CBOMClusterInfoEditReducer = (
  state = initState,
  action: { type: string; payload: CBOMEdit }
) => {
  switch (action.type) {
    case EDIT_CBOM: {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case GET_EDIT_CBOM:
      return {
        ...state,
        CBOMDtoEdit: action.payload.CBOMDtoEdit,
      };
    default:
      return state;
  }
};
