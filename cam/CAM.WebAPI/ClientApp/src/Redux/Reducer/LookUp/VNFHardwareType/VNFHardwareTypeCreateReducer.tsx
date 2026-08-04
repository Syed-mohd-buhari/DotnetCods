import { VNFHardwareTypeCreate } from "../../../../Model/LookUp/VNFHardwareType";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: VNFHardwareTypeCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const VNFHardwareTypeCreateReducer = (
  state = initState,
  action: { type: string; payload: VNFHardwareTypeCreate }
) => {
  switch (action.type) {
    case "CREATE_VNFHARDWARETYPE": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_VNFHARDWARETYPE":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
