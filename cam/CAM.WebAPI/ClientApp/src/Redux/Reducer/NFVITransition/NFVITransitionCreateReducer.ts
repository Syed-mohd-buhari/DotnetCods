import {CREATE_NFVI_TRAMSITION, GET_CREATE_NFVI_TRAMSITION, NFVITransitionCreate } from "../../../Model/NFVITransition"

const initState: NFVITransitionCreate = {
    ResultDtoCreate: null,
    NFVITransitionDtoCreate: null,
}
//const dispatch = useDispatch();


export const NFVITransitionCreateReducer = (state = initState, action: { type: string, payload: NFVITransitionCreate }) => {
    switch (action.type) {
        case CREATE_NFVI_TRAMSITION:
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case GET_CREATE_NFVI_TRAMSITION:
            return { ...state, NFVITransitionDtoCreate: action.payload.NFVITransitionDtoCreate }
        default:
            return state;
    }
}
