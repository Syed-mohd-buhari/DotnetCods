import {
  GET_FILTER_CBOM_CNF_INSTANCE_AND_CAPACITY,
  GET_GRID_CBOM_CNF_INSTANCE_AND_CAPACITY,
  CBOMCnfInstanceAndCapacityGrid,
} from "../../../Model/CBOM";

const initialState: CBOMCnfInstanceAndCapacityGrid = {
  CBOMCnfInstanceAndCapacityGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const CBOMClusterInstanceCapacityGridReducer = (
  state = initialState,
  action: { type: string; payload: CBOMCnfInstanceAndCapacityGrid }
) => {
  switch (action.type) {
    case GET_GRID_CBOM_CNF_INSTANCE_AND_CAPACITY: {
      return {
        ...state,
        CBOMCnfInstanceAndCapacityGridResult:
          action.payload.CBOMCnfInstanceAndCapacityGridResult,
      };
    }
    case GET_FILTER_CBOM_CNF_INSTANCE_AND_CAPACITY:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
