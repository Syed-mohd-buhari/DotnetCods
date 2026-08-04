import {CREATE_SETTINGS_UPDATE_PLANNED_ACTIVITY, GET_CREATE_SETTINGS_UPDATE_PLANNED_ACTIVITY, SettingsUpdatePlannedActivityCreate } from "../../../Model/SettingsUpdatePlannedActivity"

const initState: SettingsUpdatePlannedActivityCreate = {
    ResultDtoCreate: null,
    SettingsUpdatePlannedActivityDtoCreate: null,
}
//const dispatch = useDispatch();


export const SettingsUpdatePlannedActivityCreateReducer = (state = initState, action: { type: string, payload: SettingsUpdatePlannedActivityCreate }) => {
    switch (action.type) {
        case CREATE_SETTINGS_UPDATE_PLANNED_ACTIVITY:
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case GET_CREATE_SETTINGS_UPDATE_PLANNED_ACTIVITY:
            return { ...state, SettingsUpdatePlannedActivityDtoCreate: action.payload.SettingsUpdatePlannedActivityDtoCreate }
        default:
            return state;
    }
}
