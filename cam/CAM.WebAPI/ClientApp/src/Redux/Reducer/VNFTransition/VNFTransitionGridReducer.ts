import {  GET_FILTER_VNF_TRANSITION, GET_GRID_VNF_TRANSITION, VNFTransitionGrid } from "../../../Model/VNFTransition"

const initState: VNFTransitionGrid = {
    VNFTransitionGridResult: null,
    filter: null,
}
//const dispatch = useDispatch();


export const VNFTransitionGridReducer = (state = initState, action: { type: string, payload: VNFTransitionGrid }) => {
    switch (action.type) {
        case GET_GRID_VNF_TRANSITION:
            {
                return { ...state, VNFTransitionGridResult: action.payload.VNFTransitionGridResult }
            }
        case GET_FILTER_VNF_TRANSITION:
            return { ...state, filter: action.payload.filter }
        default:
            return state;
    }
}
