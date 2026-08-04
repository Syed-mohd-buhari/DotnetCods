import {  LookUpGridRule } from "../../../../Model/LookUp/LookUpGenericModel"
import {initState} from '../ActivityStatus/ActivityStatusGridReducer'
// const initState: LookUpGrid = {
//     LookUpGridResult: null,
//     LookUpGridResultAll: null,
//     filter: null,
// }
//const dispatch = useDispatch();
//const dispatch = useDispatch();


export const BudgetAvailabilityGridReducer = (state = initState, action: { type: string, payload: LookUpGridRule }) => {
    switch (action.type) {
        case "GET_GRID_BUDGET_AVAILABILITY":
            {
                return { ...state, LookUpGridResult: action.payload.LookUpGridResult }
            }
        case "GET_GRID_BUDGET_AVAILABILITY_ALL":
            {
                return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult }
            }
        case "GET_FILTER_BUDGET_AVAILABILITY":
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
