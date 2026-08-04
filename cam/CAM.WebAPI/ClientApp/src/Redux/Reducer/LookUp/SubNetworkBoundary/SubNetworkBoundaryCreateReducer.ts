import { LookUpCreateSubNetworkBoundary } from "../../../../Model/LookUp/SubnetworkBoundry";

const initState: LookUpCreateSubNetworkBoundary = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const SubNetworkBoundaryCreateReducer = (
  state = initState,
  action: { type: string; payload: LookUpCreateSubNetworkBoundary }
) => {
  switch (action.type) {
    case "CREATE_SUBNETWORK_BOUNDARY": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_SUBNETWORK_BOUNDARY":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
