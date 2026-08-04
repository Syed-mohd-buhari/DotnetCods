import { LookUpGridForSystemNames } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpGridForSystemNames = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const SystemGridReducer = (
  state = initState,
  action: { type: string; payload: LookUpGridForSystemNames }
) => {
  switch (action.type) {
    case "GET_GRID_SYSTEM_NAME": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_SYSTEM_NAME_ALL": {
      return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult };
    }
    case "GET_FILTER_SYSTEM_NAME": {
      return { ...state, filter: action.payload.filter };
    }
    default:
      return state;
  }
};
