import { LookUpCreate } from "../../../../Model/LookUp/LookUpGenericModel"
import { AssetCategoryCreate } from '../../../../Model/LookUp/AssetCategory';

const initState: AssetCategoryCreate = {
    ResultDtoCreate: null,
    LookUpDtoCreate: null,
}
//const dispatch = useDispatch();


export const AssetCategoryCreateReducer = (state = initState, action: { type: string, payload: AssetCategoryCreate }) => {
    switch (action.type) {
        case "CREATE_ASSET_CATEGORY":
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case "GET_CREATE_ASSET_CATEGORY":
            return { ...state, LookUpDtoCreate: action.payload.LookUpDtoCreate }
        default:
            return state;
    }
}
