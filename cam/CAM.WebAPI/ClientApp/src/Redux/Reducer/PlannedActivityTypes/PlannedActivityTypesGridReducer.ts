import {  GET_FILTER_PLANNED_ACTIVITY_TYPES, GET_GRID_PLANNED_ACTIVITY_TYPES, PlannedActivityTypesGrid } from "../../../Model/PlannedActivityTypes"

const initState: PlannedActivityTypesGrid = {
    PlannedActivityTypesGridResult: null,
    filter: null,
}
//const dispatch = useDispatch();


export const PlannedActivityTypesGridReducer = (state = initState, action: { type: string, payload: PlannedActivityTypesGrid }) => {
    switch (action.type) {
        case GET_GRID_PLANNED_ACTIVITY_TYPES:
            {
                return { ...state, PlannedActivityTypesGridResult: action.payload.PlannedActivityTypesGridResult }
            }
        case GET_FILTER_PLANNED_ACTIVITY_TYPES:
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
