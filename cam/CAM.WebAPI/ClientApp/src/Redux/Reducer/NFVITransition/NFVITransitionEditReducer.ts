import {EDIT_NFVI_TRAMSITION, GET_EDIT_NFVI_TRAMSITION, NFVITransitionEdit } from "../../../Model/NFVITransition"

const initState: NFVITransitionEdit = {
    NFVITransitionDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const NFVITransitionEditReducer = (state = initState, action: { type: string, payload: NFVITransitionEdit }) => {
    switch (action.type) {
        case EDIT_NFVI_TRAMSITION:
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case GET_EDIT_NFVI_TRAMSITION:
            return { ...state, NFVITransitionDtoEdit: action.payload.NFVITransitionDtoEdit }
        default:
            return state;
    }
}
