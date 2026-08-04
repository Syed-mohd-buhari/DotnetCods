import {CREATE_PLANNED_ACTIVITY, GET_CREATE_PLANNED_ACTIVITY, PlannedActivityCreate } from "../../../Model/PlannedActivity"

const initState: PlannedActivityCreate = {
    ResultDtoCreate: null,
    PlannedActivityDtoCreate: null,
}
//const dispatch = useDispatch();


export const PlannedActivityCreateReducer = (state = initState, action: { type: string, payload: PlannedActivityCreate }) => {
    switch (action.type) {
        case CREATE_PLANNED_ACTIVITY:
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case GET_CREATE_PLANNED_ACTIVITY:
            return { ...state, PlannedActivityDtoCreate: action.payload.PlannedActivityDtoCreate }
        default:
            return state;
    }
}
