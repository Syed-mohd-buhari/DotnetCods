import { LookUpEditSubNetworkBoundary } from "../../../../Model/LookUp/SubnetworkBoundry";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: LookUpEditSubNetworkBoundary = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const SubNetworkBoundaryEditReducer = (
  state = initState,
  action: { type: string; payload: LookUpEditSubNetworkBoundary }
) => {
  switch (action.type) {
    case "EDIT_SUBNETWORK_BOUNDARY": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_SUBNETWORK_BOUNDARY":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
