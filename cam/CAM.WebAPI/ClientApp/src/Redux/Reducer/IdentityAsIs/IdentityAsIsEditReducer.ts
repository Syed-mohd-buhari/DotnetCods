import {
  EDIT_IDENTiTYASIS,
  GET_EDIT_IDENTiTYASIS,
  IdentityAsIsEdit,
} from "../../../Model/LookUp/Identities";

const initState: IdentityAsIsEdit = {
  IdentityAsIsDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const IdentityAsIsEditReducer = (
  state = initState,
  action: { type: string; payload: IdentityAsIsEdit }
) => {
  switch (action.type) {
    case EDIT_IDENTiTYASIS: {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case GET_EDIT_IDENTiTYASIS:
      return {
        ...state,
        IdentityAsIsDtoEdit: action.payload.IdentityAsIsDtoEdit,
      };
    default:
      return state;
  }
};
