import {CREATE_VNF_TRANSITION, GET_CREATE_VNF_TRANSITION, VNFTransitionCreate } from "../../../Model/VNFTransition"

const initState: VNFTransitionCreate = {
    ResultDtoCreate: null,
    VNFTransitionDtoCreate: null,
}
//const dispatch = useDispatch();


export const VNFTransitionCreateReducer = (state = initState, action: { type: string, payload: VNFTransitionCreate }) => {
    switch (action.type) {
        case CREATE_VNF_TRANSITION:
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case GET_CREATE_VNF_TRANSITION:
            return { ...state, VNFTransitionDtoCreate: action.payload.VNFTransitionDtoCreate }
        default:
            return state;
    }
}
