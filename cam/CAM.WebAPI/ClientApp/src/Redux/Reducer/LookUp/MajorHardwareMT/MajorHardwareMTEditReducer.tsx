import { MajorHardwareMTEdit } from "../../../../Model/LookUp/MajorHardwareMT";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: MajorHardwareMTEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const MajorHardwareMTEditReducer = (
  state = initState,
  action: { type: string; payload: MajorHardwareMTEdit }
) => {
  switch (action.type) {
    case "EDIT_MAJORHARDWAREMT": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_MAJORHARDWAREMT":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
