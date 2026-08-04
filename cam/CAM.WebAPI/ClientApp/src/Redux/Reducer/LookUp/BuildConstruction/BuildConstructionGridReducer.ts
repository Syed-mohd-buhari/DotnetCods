import { LookUpGrid, LookUpGridRule } from "../../../../Model/LookUp/LookUpGenericModel"
import {initState} from '../ActivityStatus/ActivityStatusGridReducer'
// const initState: LookUpGrid = {
//     LookUpGridResult: null,
//     LookUpGridResultAll: null,
//     filter: null,
// }
//const dispatch = useDispatch();
//const dispatch = useDispatch();


export const BuildConstructionGridReducer = (state = initState, action: { type: string, payload: LookUpGridRule }) => {
    switch (action.type) {
        case "GET_GRID_BUILD_CONSTRUCTION":
            {
                return { ...state, LookUpGridResult: action.payload.LookUpGridResult }
            }
        case "GET_GRID_BUILD_CONSTRUCTION_ALL":
            {
                return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult }
            }
        case "GET_FILTER_BUILD_CONSTRUCTION":
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
