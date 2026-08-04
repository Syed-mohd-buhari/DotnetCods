import {
  InfraClusterEdit,
  GET_EDIT_INFRA_CLUSTER,
  EDIT_INFRA_CLUSTER,
} from "../../../Model/InfraCluster";

const initState: InfraClusterEdit = {
  ResultDtoEdit: null,
  InfraClusterDtoEdit: null,
};
//const dispatch = useDispatch();

export const InfraClusterEditReducer = (
  state = initState,
  action: { type: string; payload: InfraClusterEdit }
) => {
  switch (action.type) {
    case EDIT_INFRA_CLUSTER: {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case GET_EDIT_INFRA_CLUSTER:
      return {
        ...state,
        InfraClusterDtoEdit: action.payload.InfraClusterDtoEdit,
      };
    default:
      return state;
  }
};
