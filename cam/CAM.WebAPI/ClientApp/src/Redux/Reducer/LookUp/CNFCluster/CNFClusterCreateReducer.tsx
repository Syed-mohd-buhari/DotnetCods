import { CNFClusterCreate } from "../../../../Model/LookUp/CNFCluster";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: CNFClusterCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const CNFClusterCreateReducer = (
  state = initState,
  action: { type: string; payload: CNFClusterCreate }
) => {
  switch (action.type) {
    case "CREATE_CNFCLUSTER": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_CNFCLUSTER":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
