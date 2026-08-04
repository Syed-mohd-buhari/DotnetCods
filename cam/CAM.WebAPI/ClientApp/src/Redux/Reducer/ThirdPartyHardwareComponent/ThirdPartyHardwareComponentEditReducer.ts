import {EDIT_THIRD_PARTY_HARDWARE_COMPONENT, GET_EDIT_THIRD_PARTY_HARDWARE_COMPONENT, ThirdPartyHardwareComponentEdit } from "../../../Model/ThirdPartyHardwareComponent"

const initState: ThirdPartyHardwareComponentEdit = {
    ThirdPartyHardwareComponentDtoEdit: null,
    ResultDtoEdit:null
}
//const dispatch = useDispatch();


export const ThirdPartyHardwareComponentEditReducer = (state = initState, action: { type: string, payload: ThirdPartyHardwareComponentEdit }) => {
    switch (action.type) {
        case EDIT_THIRD_PARTY_HARDWARE_COMPONENT:
            {
                return { ...state, ResultDtoEdit: action.payload.ResultDtoEdit }
            }
        case GET_EDIT_THIRD_PARTY_HARDWARE_COMPONENT:
            return { ...state, ThirdPartyHardwareComponentDtoEdit: action.payload.ThirdPartyHardwareComponentDtoEdit }
        default:
            return state;
    }
}
