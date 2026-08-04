import { VNFClusterNameCreate } from "../../../../Model/LookUp/VNFClusterName";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: VNFClusterNameCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const VNFClusterNameCreateReducer = (
  state = initState,
  action: { type: string; payload: VNFClusterNameCreate }
) => {
  switch (action.type) {
    case "CREATE_VNFCLUSTERNAME": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_VNFCLUSTERNAME":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
