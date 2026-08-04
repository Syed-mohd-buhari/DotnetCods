import { LookUpEdit } from "../../../../Model/LookUp/LookUpGenericModel"
import { AssetCategoryEdit } from '../../../../Model/LookUp/AssetCategory';

const initState: AssetCategoryEdit = {
    LookUpDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const AssetCategoryEditReducer = (state = initState, action: { type: string, payload: AssetCategoryEdit }) => {
    switch (action.type) {
        case "EDIT_ASSET_CATEGORY":
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case "GET_EDIT_ASSET_CATEGORY":
            return { ...state, LookUpDtoEdit: action.payload.LookUpDtoEdit }
        default:
            return state;
    }
}
