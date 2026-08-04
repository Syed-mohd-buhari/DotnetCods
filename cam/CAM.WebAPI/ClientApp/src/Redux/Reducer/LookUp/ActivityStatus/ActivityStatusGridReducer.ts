import { LookUpGrid, LookUpGridRule } from "../../../../Model/LookUp/LookUpGenericModel"

export const initState: LookUpGridRule = {
    LookUpGridResult: null,
    LookUpGridResultAll: null,
    filter: null,
}
//const dispatch = useDispatch();


export const ActivityStatusGridReducer = (state = initState, action: { type: string, payload: LookUpGridRule }) => {
    switch (action.type) {
        case "GET_GRID_ACTIVITY_STATUS":
            {

                return { ...state, LookUpGridResult: action.payload.LookUpGridResult }
            }
        case "GET_GRID_ACTIVITY_STATUS_ALL":
            {

                return { ...state, LookUpGridResultAll: action.payload.LookUpGridResult }
            }
        case "GET_FILTER_ACTIVITY_STATUS":
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
