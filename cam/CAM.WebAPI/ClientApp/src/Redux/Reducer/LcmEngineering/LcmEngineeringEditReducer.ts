import {EDIT_LCM_ENGINEERING, GET_EDIT_LCM_ENGINEERING, LcmEngineeringEdit } from "../../../Model/LcmEngineering"

const initState: LcmEngineeringEdit = {
    LcmEngineeringDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const LcmEngineeringEditReducer = (state = initState, action: { type: string, payload: LcmEngineeringEdit }) => {
    switch (action.type) {
        case EDIT_LCM_ENGINEERING:
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case GET_EDIT_LCM_ENGINEERING:
            return { ...state, LcmEngineeringDtoEdit: action.payload.LcmEngineeringDtoEdit }
        default:
            return state;
    }
}
