import { ResultDto } from "../../../Model/CommonModels"
import { DELETE_PLANNED_ACTIVITY_TYPES, RESTORE_PLANNED_ACTIVITY_TYPES } from "../../../Model/PlannedActivityTypes"

const initState: ResultDto = {
    data: undefined,
    info: undefined,
    warning: undefined
}
//const dispatch = useDispatch();


export const PlannedActivityTypesDeleteReducer = (state = initState, action: { type: string, payload: ResultDto }) => {
    switch (action.type) {
        case DELETE_PLANNED_ACTIVITY_TYPES:
        case RESTORE_PLANNED_ACTIVITY_TYPES:
            {
                return { ...state, ResultDto: action.payload }
            }
        default:
            return state;
    }
}
