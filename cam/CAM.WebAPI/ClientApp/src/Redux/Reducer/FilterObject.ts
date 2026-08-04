import { useSelector } from "react-redux";
import { RootState } from "../Store/rootStore";
import { LookUpGraphFilter } from "../../Model/LookUp/LookUpGenericModel";

const initialState: LookUpGraphFilter = {
  FilterData: null,
};

export const GraphFilterObject = (
  state = initialState,
  action: { type: string; payload: LookUpGraphFilter }
) => {
  switch (action.type) {
    case "GET_GRAPH_FILTER":
      return { ...state, FilterData: action.payload.FilterData };
    default:
      return state;
  }
};
