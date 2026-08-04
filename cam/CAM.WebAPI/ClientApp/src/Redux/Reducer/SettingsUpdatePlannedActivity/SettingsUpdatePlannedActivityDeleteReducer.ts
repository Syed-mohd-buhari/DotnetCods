import { ResultDto } from "../../../Model/CommonModels"
import { DELETE_SETTINGS_UPDATE_PLANNED_ACTIVITY, RESTORE_SETTINGS_UPDATE_PLANNED_ACTIVITY } from "../../../Model/SettingsUpdatePlannedActivity"

const initState: ResultDto = {
    data: undefined,
    info: undefined,
    warning: undefined
}
//const dispatch = useDispatch();


export const SettingsUpdatePlannedActivityDeleteReducer = (state = initState, action: { type: string, payload: ResultDto }) => {
    switch (action.type) {
        case DELETE_SETTINGS_UPDATE_PLANNED_ACTIVITY:
        case RESTORE_SETTINGS_UPDATE_PLANNED_ACTIVITY:
            {
                return { ...state, ResultDto: action.payload }
            }
        default:
            return state;
    }
}
