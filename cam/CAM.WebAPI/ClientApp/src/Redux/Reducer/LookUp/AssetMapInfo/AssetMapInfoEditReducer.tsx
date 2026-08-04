import { AssetMapInfoEdit } from "../../../../Model/LookUp/AssetMapInfo";
import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: AssetMapInfoEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const AssetMapInfoEditReducer = (
  state = initState,
  action: { type: string; payload: AssetMapInfoEdit }
) => {
  switch (action.type) {
    case "EDIT_ASSETMAPINFO": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_ASSETMAPINFO":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
