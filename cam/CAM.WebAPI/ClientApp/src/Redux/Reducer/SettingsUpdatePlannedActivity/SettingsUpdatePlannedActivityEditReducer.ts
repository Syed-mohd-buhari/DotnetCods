import {EDIT_SETTINGS_UPDATE_PLANNED_ACTIVITY, GET_EDIT_SETTINGS_UPDATE_PLANNED_ACTIVITY, SettingsUpdatePlannedActivityEdit } from "../../../Model/SettingsUpdatePlannedActivity"

const initState: SettingsUpdatePlannedActivityEdit = {
    SettingsUpdatePlannedActivityDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const SettingsUpdatePlannedActivityEditReducer = (state = initState, action: { type: string, payload: SettingsUpdatePlannedActivityEdit }) => {
    switch (action.type) {
        case EDIT_SETTINGS_UPDATE_PLANNED_ACTIVITY:
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case GET_EDIT_SETTINGS_UPDATE_PLANNED_ACTIVITY:
            return { ...state, SettingsUpdatePlannedActivityDtoEdit: action.payload.SettingsUpdatePlannedActivityDtoEdit }
        default:
            return state;
    }
}
