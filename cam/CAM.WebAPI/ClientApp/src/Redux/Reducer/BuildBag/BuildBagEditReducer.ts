import {
  EDIT_BUILD_BAG,
  GET_EDIT_BUILD_BAG,
  BuildBagEdit,
} from "../../../Model/BuildBag";

const initState: BuildBagEdit = {
  BuildBagDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const BuildBagEditReducer = (
  state = initState,
  action: { type: string; payload: BuildBagEdit }
) => {
  switch (action.type) {
    case EDIT_BUILD_BAG: {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case GET_EDIT_BUILD_BAG:
      return {
        ...state,
        BuildBagDtoEdit: action.payload.BuildBagDtoEdit,
      };
    default:
      return state;
  }
};
