import { AssetMapInfoCreate } from "../../../../Model/LookUp/AssetMapInfo";
import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";

const initState: AssetMapInfoCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const AssetMapInfoCreateReducer = (
  state = initState,
  action: { type: string; payload: AssetMapInfoCreate }
) => {
  switch (action.type) {
    case "CREATE_ASSETMAPINFO": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_ASSETMAPINFO":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
