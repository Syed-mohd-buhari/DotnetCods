import {
  GET_GRID_RESOURCE_KEY_MASTER,
  GET_FILTER_NETWORK_ELEMENT_AS_IS,
  DCFLifeCycleGrid,
} from "../../../Model/DCFLifeCycle";

const initState: DCFLifeCycleGrid = {
  DCFLifeCycleGridResult: null,
  filter: null,
};
//const dispatch = useDispatch();

//Added newly for New Network Element

export const DCFLifeCycleGridReducer = (
  state = initState,
  action: { type: string; payload: DCFLifeCycleGrid }
) => {
  //console.log("Tems Reducer 2");
  switch (action.type) {
    case GET_GRID_RESOURCE_KEY_MASTER: {
      return {
        ...state,
        DCFLifeCycleGridResult: action.payload.DCFLifeCycleGridResult,
      };
    }
    case GET_FILTER_NETWORK_ELEMENT_AS_IS:
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
