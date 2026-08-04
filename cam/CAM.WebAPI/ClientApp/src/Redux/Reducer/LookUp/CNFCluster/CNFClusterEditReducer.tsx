import { CNFClusterEdit } from "../../../../Model/LookUp/CNFCluster";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFClusterEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const CNFClusterEditReducer = (
  state = initState,
  action: { type: string; payload: CNFClusterEdit }
) => {
  switch (action.type) {
    case "EDIT_CNFCLUSTER": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_CNFCLUSTER":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
