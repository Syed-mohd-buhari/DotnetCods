import {
  CREATE_BUILD_BAG,
  GET_CREATE_BUILD_BAG,
  BuildBagCreate,
} from "../../../Model/BuildBag";

const initState: BuildBagCreate = {
  ResultDtoCreate: null,
  BuildBagDtoCreate: null,
};
//const dispatch = useDispatch();

export const BuildBagCreateReducer = (
  state = initState,
  action: { type: string; payload: BuildBagCreate }
) => {
  switch (action.type) {
    case CREATE_BUILD_BAG: {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case GET_CREATE_BUILD_BAG:
      return {
        ...state,
        BuildBagDtoCreate: action.payload.BuildBagDtoCreate,
      };
    default:
      return state;
  }
};
