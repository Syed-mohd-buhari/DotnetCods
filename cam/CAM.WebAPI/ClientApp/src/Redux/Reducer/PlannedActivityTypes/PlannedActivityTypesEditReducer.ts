import {EDIT_PLANNED_ACTIVITY_TYPES, GET_EDIT_PLANNED_ACTIVITY_TYPES, PlannedActivityTypesEdit } from "../../../Model/PlannedActivityTypes"

const initState: PlannedActivityTypesEdit = {
    PlannedActivityTypesDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const PlannedActivityTypesEditReducer = (state = initState, action: { type: string, payload: PlannedActivityTypesEdit }) => {
    switch (action.type) {
        case EDIT_PLANNED_ACTIVITY_TYPES:
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case GET_EDIT_PLANNED_ACTIVITY_TYPES:
            return { ...state, PlannedActivityTypesDtoEdit: action.payload.PlannedActivityTypesDtoEdit }
        default:
            return state;
    }
}
