import {  GET_FILTER_NFVI_TRAMSITION, GET_GRID_NFVI_TRAMSITION, NFVITransitionGrid } from "../../../Model/NFVITransition"

const initState: NFVITransitionGrid = {
    NFVITransitionGridResult: null,
    filter: null,
}
//const dispatch = useDispatch();


export const NFVITransitionGridReducer = (state = initState, action: { type: string, payload: NFVITransitionGrid }) => {
    switch (action.type) {
        case GET_GRID_NFVI_TRAMSITION:
            {
                return { ...state, NFVITransitionGridResult: action.payload.NFVITransitionGridResult }
            }
        case GET_FILTER_NFVI_TRAMSITION:
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
