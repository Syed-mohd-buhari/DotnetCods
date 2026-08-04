import { AssetCategoryGrid } from "../../../../Model/LookUp/AssetCategory"
import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel"
const initState: AssetCategoryGrid = {
    LookUpGridResult: null,
    LookUpGridResultAll: null,
    filter: null,
}
//const dispatch = useDispatch();


export const AssetCategoryGridReducer = (state = initState, action: { type: string, payload: AssetCategoryGrid }) => {
    switch (action.type) {
        case "GET_GRID_ASSET_CATEGORY":
            {
                return { ...state, LookUpGridResult: action.payload.LookUpGridResult }
            }
        case "GET_GRID_ASSET_CATEGORY_ALL":
            {
                return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult }
            }
        case "GET_FILTER_ASSET_CATEGORY":
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
