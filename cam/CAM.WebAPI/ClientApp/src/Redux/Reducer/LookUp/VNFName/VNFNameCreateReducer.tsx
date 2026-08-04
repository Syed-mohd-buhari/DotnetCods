import { VNFNameCreate } from "../../../../Model/LookUp/VNFName";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: VNFNameCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const VNFNameCreateReducer = (
  state = initState,
  action: { type: string; payload: VNFNameCreate }
) => {
  switch (action.type) {
    case "CREATE_VNFNAME": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_VNFNAME":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
