import {CREATE_PLANNED_ACTIVITY_TYPES, GET_CREATE_PLANNED_ACTIVITY_TYPES, PlannedActivityTypesCreate } from "../../../Model/PlannedActivityTypes"

const initState: PlannedActivityTypesCreate = {
    ResultDtoCreate: null,
    PlannedActivityTypesDtoCreate: null,
}
//const dispatch = useDispatch();


export const PlannedActivityTypesCreateReducer = (state = initState, action: { type: string, payload: PlannedActivityTypesCreate }) => {
    switch (action.type) {
        case CREATE_PLANNED_ACTIVITY_TYPES:
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case GET_CREATE_PLANNED_ACTIVITY_TYPES:
            return { ...state, PlannedActivityTypesDtoCreate: action.payload.PlannedActivityTypesDtoCreate }
        default:
            return state;
    }
}
