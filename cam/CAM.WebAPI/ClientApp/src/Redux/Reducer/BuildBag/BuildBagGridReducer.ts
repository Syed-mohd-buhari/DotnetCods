import {
  GET_FILTER_BUILD_BAG,
  GET_GRID_BUILD_BAG,
  BuildBagGrid,
} from "../../../Model/BuildBag";

const initState: BuildBagGrid = {
  BuildBagGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const BuildBagGridReducer = (
  state = initState,
  action: { type: string; payload: BuildBagGrid }
) => {
  switch (action.type) {
    case GET_GRID_BUILD_BAG: {
      return {
        ...state,
        BuildBagGridResult: action.payload.BuildBagGridResult,
      };
    }
    case GET_FILTER_BUILD_BAG:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
