import {  GET_FILTER_SETTINGS_UPDATE_PLANNED_ACTIVITY, GET_GRID_SETTINGS_UPDATE_PLANNED_ACTIVITY, SettingsUpdatePlannedActivityGrid } from "../../../Model/SettingsUpdatePlannedActivity"

const initState: SettingsUpdatePlannedActivityGrid = {
    SettingsUpdatePlannedActivityGridResult: null,
    filter: null,
}
//const dispatch = useDispatch();


export const SettingsUpdatePlannedActivityGridReducer = (state = initState, action: { type: string, payload: SettingsUpdatePlannedActivityGrid }) => {
    switch (action.type) {
        case GET_GRID_SETTINGS_UPDATE_PLANNED_ACTIVITY:
            {
                return { ...state, SettingsUpdatePlannedActivityGridResult: action.payload.SettingsUpdatePlannedActivityGridResult }
            }
        case GET_FILTER_SETTINGS_UPDATE_PLANNED_ACTIVITY:
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
