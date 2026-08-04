import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";
import { CriticalAssetTypeCreate } from "../../../../Model/LookUp/CriticalAssetType";

const initState: CriticalAssetTypeCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const CriticalAssetTypeCreateReducer = (
  state = initState,
  action: { type: string; payload: CriticalAssetTypeCreate }
) => {
  switch (action.type) {
    case "CREATE_CRITICAL_ASSET_TYPE": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_CRITICAL_ASSET_TYPE":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
