import { VNFHardwareTypeEdit } from "../../../../Model/LookUp/VNFHardwareType";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: VNFHardwareTypeEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const VNFHardwareTypeEditReducer = (
  state = initState,
  action: { type: string; payload: VNFHardwareTypeEdit }
) => {
  switch (action.type) {
    case "EDIT_VNFHARDWARETYPE": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_VNFHARDWARETYPE":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
