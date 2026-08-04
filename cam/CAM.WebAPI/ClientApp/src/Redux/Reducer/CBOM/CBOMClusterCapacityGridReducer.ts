import {
  GET_FILTER_CBOM_CNF_CAPACITY,
  GET_GRID_CBOM_CNF_CAPACITY,
  CBOMVnfCapacityGrid,
} from "../../../Model/CBOM";

const initialState: CBOMVnfCapacityGrid = {
  CBOMCnfCapacityGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

export const CBOMClusterCapacityGridReducer = (
  state = initialState,
  action: { type: string; payload: CBOMVnfCapacityGrid }
) => {
  switch (action.type) {
    case GET_GRID_CBOM_CNF_CAPACITY: {
      return {
        ...state,
        CBOMCnfCapacityGridResult: action.payload.CBOMCnfCapacityGridResult,
      };
    }
    case GET_FILTER_CBOM_CNF_CAPACITY:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
