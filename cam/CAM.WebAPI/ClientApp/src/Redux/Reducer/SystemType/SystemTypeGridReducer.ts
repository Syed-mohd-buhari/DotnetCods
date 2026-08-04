import { GET_GRID_SYSTEM_TYPE, GET_FILTER_SYSTEM_TYPE, SystemTypeGrid } from "../../../Model/SystemTypeModel"

const initState: SystemTypeGrid = {
    SystemTypeGridResult: null,
    filter: null,
}
//const dispatch = useDispatch();


export const SystemTypeGridReducer = (state = initState, action: { type: string, payload: SystemTypeGrid }) => {
    switch (action.type) {
        case GET_GRID_SYSTEM_TYPE:
            {
                return { ...state, SystemTypeGridResult: action.payload.SystemTypeGridResult }
            }
        case GET_FILTER_SYSTEM_TYPE:
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
