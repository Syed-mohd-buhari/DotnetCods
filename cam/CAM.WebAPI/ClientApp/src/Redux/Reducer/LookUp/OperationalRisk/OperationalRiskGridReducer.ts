import { LookUpGridRisk } from "../../../../Model/LookUp/OperationalRisk"
import {initState} from '../ActivityStatus/ActivityStatusGridReducer'
// const initState: LookUpGrid = {
//     LookUpGridResult: null,
//     LookUpGridResultAll: null,
//     filter: null,
// }
//const dispatch = useDispatch();
//const dispatch = useDispatch();


export const OperationalRiskGridReducer = (state = initState, action: { type: string, payload: LookUpGridRisk }) => {
    switch (action.type) {
        case "GET_GRID_OPERATIONAL_RISK":
            {
                return { ...state, LookUpGridResult: action.payload.LookUpGridRiskResult }
            }
        case "GET_GRID_OPERATIONAL_RISK_ALL":
            {
                return { ...state, LookUpGridResultAll: action.payload.LookUpGridRiskResult }
            }
        case "GET_FILTER_OPERATIONAL_RISK":
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
