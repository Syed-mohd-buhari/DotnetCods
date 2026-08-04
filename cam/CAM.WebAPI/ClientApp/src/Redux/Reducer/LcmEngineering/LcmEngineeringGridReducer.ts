import { GET_GRID_LCM_ENGINEERING, GET_FILTER_LCM_ENGINEERING, LcmEngineeringGrid } from "../../../Model/LcmEngineering"

const initState: LcmEngineeringGrid = {
    LcmEngineeringGridResult: null,
    filter: null,
}
//const dispatch = useDispatch();


export const LcmEngineeringGridReducer = (state = initState, action: { type: string, payload: LcmEngineeringGrid }) => {
    switch (action.type) {
        case GET_GRID_LCM_ENGINEERING:
            {
                return { ...state, LcmEngineeringGridResult: action.payload.LcmEngineeringGridResult }
            }
        case GET_FILTER_LCM_ENGINEERING:
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
