import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel";
import { AssetTypeCreate } from "../../../../Model/LookUp/AssetType";

const initState: AssetTypeCreate = {
  ResultDtoCreate: null,
  LookUpDtoCreate: null,
};
//const dispatch = useDispatch();

export const AssetTypeCreateReducer = (
  state = initState,
  action: { type: string; payload: AssetTypeCreate }
) => {
  switch (action.type) {
    case "CREATE_ASSET_TYPE": {
      return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate };
    }
    case "GET_CREATE_ASSET_TYPE":
      return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate };
    default:
      return state;
  }
};
