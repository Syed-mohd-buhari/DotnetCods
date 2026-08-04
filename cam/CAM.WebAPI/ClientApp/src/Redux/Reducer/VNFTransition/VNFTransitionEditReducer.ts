import {EDIT_VNF_TRANSITION, GET_EDIT_VNF_TRANSITION, VNFTransitionEdit } from "../../../Model/VNFTransition"

const initState: VNFTransitionEdit = {
    VNFTransitionDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const VNFTransitionEditReducer = (state = initState, action: { type: string, payload: VNFTransitionEdit }) => {
    switch (action.type) {
        case EDIT_VNF_TRANSITION:
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case GET_EDIT_VNF_TRANSITION:
            return { ...state, VNFTransitionDtoEdit: action.payload.VNFTransitionDtoEdit }
        default:
            return state;
    }
}
