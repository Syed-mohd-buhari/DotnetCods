import { LookUpGrid } from "../../../../Model/LookUp/LookUpGenericModel"
import {initState} from '../ActivityStatus/ActivityStatusGridReducer'
// const initState: LookUpGrid = {
//     LookUpGridResult: null,
//     LookUpGridResultAll: null,
//     filter: null,
// }
//const dispatch = useDispatch();
//const dispatch = useDispatch();


export const SubDomainSpocGridReducer = (state = initState, action: { type: string, payload: LookUpGrid }) => {
    switch (action.type) {
        case "GET_GRID_SUB_DOMAIN_SPOC":
            {
                return { ...state, LookUpGridResult: action.payload.LookUpGridResult }
            }
        case "GET_GRID_SUB_DOMAIN_SPOC_ALL":
            {
                return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult }
            }
        case "GET_FILTER_SUB_DOMAIN_SPOC":
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
