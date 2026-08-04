import { VNFClusterNameEdit } from "../../../../Model/LookUp/VNFClusterName";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: VNFClusterNameEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const VNFClusterNameEditReducer = (
  state = initState,
  action: { type: string; payload: VNFClusterNameEdit }
) => {
  switch (action.type) {
    case "EDIT_VNFCLUSTERNAME": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_VNFCLUSTERNAME":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
