import { MajorHardwareMTGrid } from "../../../../Model/LookUp/MajorHardwareMT";
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: MajorHardwareMTGrid = {
  LookUpGridResult: null,
  LookUpGridResultAll: null,
  filter: null,
};

export const MajorHardwareMTGridReducer = (
  state = initState,
  action: { type: string; payload: MajorHardwareMTGrid }
) => {
  switch (action.type) {
    case "GET_GRID_MAJORHARDWAREMT": {
      return { ...state, LookUpGridResult: action.payload.LookUpGridResult };
    }
    case "GET_GRID_MAJORHARDWAREMT_ALL": {
      return {
        ...state,
        LookUpGridResultAll: action.payload.LookUpGridResult,
      };
    }
    case "GET_FILTER_MAJORHARDWAREMT":
      return { ...state, filter: action.payload.filter };
    default:
      return state;
  }
};
