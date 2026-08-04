import {
  CREATE_IDENTITYASIS,
  GET_CREATE_IDENTiTYASIS,
  IdentityAsIsCreate,
  IdentityAsIsDtoCreate,
} from "../../../Model/LookUp/Identities";

const initState: IdentityAsIsCreate = {
  ResultDtoCreate: null,
  IdentityAsIsDtoCreate: null,
};
//const dispatch = useDispatch();

export const IdentityAsIsCreateReducer = (
  state = initState,
  action: { type: string; payload: IdentityAsIsCreate }
) => {
  switch (action.type) {
    case CREATE_IDENTITYASIS:
      alert("ss");
      {
        return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
      }
    case GET_CREATE_IDENTiTYASIS:
      console.log(
        action.payload.IdentityAsIsDtoCreate,
        "action.payload.IdentityAsIsDtoCreate "
      );
      return {
        ...state,
        IdentityAsIsDtoCreate: action.payload.IdentityAsIsDtoCreate,
      };
    default:
      return state;
  }
};
