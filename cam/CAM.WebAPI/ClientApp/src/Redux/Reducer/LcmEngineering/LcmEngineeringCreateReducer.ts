import {CREATE_LCM_ENGINEERING, GET_CREATE_LCM_ENGINEERING, LcmEngineeringCreate } from "../../../Model/LcmEngineering"

const initState: LcmEngineeringCreate = {
    ResultDtoCreate: null,
    LcmEngineeringDtoCreate: null,
}
//const dispatch = useDispatch();


export const LcmEngineeringCreateReducer = (state = initState, action: { type: string, payload: LcmEngineeringCreate }) => {
    switch (action.type) {
        case CREATE_LCM_ENGINEERING:
            {
                return { ...state, ResultDtoCreate: action.payload.ResultDtoCreate }
            }
        case GET_CREATE_LCM_ENGINEERING:
            return { ...state, LcmEngineeringDtoCreate: action.payload.LcmEngineeringDtoCreate }
        default:
            return state;
    }
}
