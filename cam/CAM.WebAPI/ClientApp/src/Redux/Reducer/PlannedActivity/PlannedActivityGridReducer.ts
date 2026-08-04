import { GET_GRID_PLANNED_ACTIVITY, GET_FILTER_PLANNED_ACTIVITY, PlannedActivityGrid } from "../../../Model/PlannedActivity"

const initState: PlannedActivityGrid = {
    PlannedActivityGridResult: null,
    filter: null,
}
//const dispatch = useDispatch();


export const PlannedActivityGridReducer = (state = initState, action: { type: string, payload: PlannedActivityGrid }) => {
    switch (action.type) {
        case GET_GRID_PLANNED_ACTIVITY:
            {
                return { ...state, PlannedActivityGridResult: action.payload.PlannedActivityGridResult }
            }
        case GET_FILTER_PLANNED_ACTIVITY:
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
