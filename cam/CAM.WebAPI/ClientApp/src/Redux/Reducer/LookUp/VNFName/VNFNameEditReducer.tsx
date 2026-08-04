import { VNFNameEdit } from "../../../../Model/LookUp/VNFName";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: VNFNameEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const VNFNameEditReducer = (
  state = initState,
  action: { type: string; payload: VNFNameEdit }
) => {
  switch (action.type) {
    case "EDIT_VNFNAME": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_VNFNAME":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
