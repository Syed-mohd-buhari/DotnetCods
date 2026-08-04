import { MajorHardwareMTCreate } from "../../../../Model/LookUp/MajorHardwareMT";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: MajorHardwareMTCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const MajorHardwareMTCreateReducer = (
  state = initState,
  action: { type: string; payload: MajorHardwareMTCreate }
) => {
  switch (action.type) {
    case "CREATE_MAJORHARDWAREMT": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_MAJORHARDWAREMT":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
