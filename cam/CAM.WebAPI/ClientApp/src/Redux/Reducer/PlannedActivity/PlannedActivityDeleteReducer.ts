import { ResultDto } from "../../../Model/CommonModels"
import { DELETE_PLANNED_ACTIVITY, RESTORE_PLANNED_ACTIVITY } from "../../../Model/PlannedActivity"

const initState: ResultDto = {
    data: undefined,
    info: undefined,
    warning: undefined
}
//const dispatch = useDispatch();


export const PlannedActivityDeleteReducer = (state = initState, action: { type: string, payload: ResultDto }) => {
    switch (action.type) {
        case DELETE_PLANNED_ACTIVITY:
        case RESTORE_PLANNED_ACTIVITY:
            {
                return { ...state, ResultDto: action.payload }
            }
        default:
            return state;
    }
}
