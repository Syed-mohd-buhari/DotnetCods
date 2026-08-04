import { TypeGrid } from "../../../../Model/LookUp/Type";

const initState: TypeGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};
//const dispatch = useDispatch();
//const dispatch = useDispatch();

export const TypeGridReducer = (
  state = initState,
  action: { type: string; payload: TypeGrid }
) => {
  switch (action.type) {
    case "GET_GRID_TYPE": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_TYPE_ALL": {

      return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult };
    }
    case "GET_FILTER_TYPE":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
