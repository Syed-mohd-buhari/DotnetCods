import {EDIT_PLANNED_ACTIVITY, GET_EDIT_PLANNED_ACTIVITY, PlannedActivityEdit } from "../../../Model/PlannedActivity"

const initState: PlannedActivityEdit = {
    PlannedActivityDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const PlannedActivityEditReducer = (state = initState, action: { type: string, payload: PlannedActivityEdit }) => {
    switch (action.type) {
        case EDIT_PLANNED_ACTIVITY:
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case GET_EDIT_PLANNED_ACTIVITY:
            return { ...state, PlannedActivityDtoEdit: action.payload.PlannedActivityDtoEdit }
        default:
            return state;
    }
}
