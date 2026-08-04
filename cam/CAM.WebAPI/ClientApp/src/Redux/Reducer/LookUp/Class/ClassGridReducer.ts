import { ClassGrid } from "../../../../Model/LookUp/Class";

const initState: ClassGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};
//const dispatch = useDispatch();
//const dispatch = useDispatch();

export const ClassGridReducer = (
  state = initState,
  action: { type: string; payload: ClassGrid }
) => {
  switch (action.type) {
    case "GET_GRID_CLASS": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_CLASS_ALL": {
      return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult };
    }
    case "GET_FILTER_CLASS":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
