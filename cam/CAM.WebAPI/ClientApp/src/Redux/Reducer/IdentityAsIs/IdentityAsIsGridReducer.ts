import {
  GET_GRID_IDENTITYASIS,
  GET_FILTER_IDENTITYASIS,
  IdentityAsIsGrid,
} from "../../../Model/LookUp/Identities";

const initState: IdentityAsIsGrid = {
  IdentityAsIsGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const IdentityAsIsGridReducer = (
  state = initState,
  action: { type: string; payload: IdentityAsIsGrid }
) => {
  switch (action.type) {
    case GET_GRID_IDENTITYASIS: {
      return {
        ...state,
        IdentityAsIsGridResult: action.payload.IdentityAsIsGridResult,
      };
    }
    case GET_FILTER_IDENTITYASIS:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
