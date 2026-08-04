import {
  InfraClusterCreate,
  GET_CREATE_INFRA_CLUSTER,
  CREATE_INFRA_CLUSTER,
} from "../../../Model/InfraCluster";

const initState: InfraClusterCreate = {
  ResultDtoCreate: null,
  InfraClusterDtoCreate: null,
};
//const dispatch = useDispatch();

export const InfraClusterCreateReducer = (
  state = initState,
  action: { type: string; payload: InfraClusterCreate }
) => {
  switch (action.type) {
    case CREATE_INFRA_CLUSTER: {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case GET_CREATE_INFRA_CLUSTER:
      return {
        ...state,
        InfraClusterDtoCreate: action.payload.InfraClusterDtoCreate,
      };
    default:
      return state;
  }
};
