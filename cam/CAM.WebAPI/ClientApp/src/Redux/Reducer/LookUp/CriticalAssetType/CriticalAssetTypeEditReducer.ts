import { AssetTypeEdit } from "../../../../Model/LookUp/AssetType";

const initState: AssetTypeEdit = {
  LookUpDtoEdit: null,
  ResultDtoEdit: null,
};
//const dispatch = useDispatch();

export const CriticalAssetTypeEditReducer = (
  state = initState,
  action: { type: string; payload: AssetTypeEdit }
) => {
  switch (action.type) {
    case "EDIT_CRITICAL_ASSET_TYPE": {
      return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit };
    }
    case "GET_EDIT_CRITICAL_ASSET_TYPE":
      return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit };
    default:
      return state;
  }
};
